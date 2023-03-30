using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace Houston.Infrastructure.Services {
	public class DockerContainerBuilderService : IContainerBuilderService {
		private readonly DockerClient _client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
		private readonly ContainerBuilder _container = new();

		public IContainerBuilderService AddContainer(string containerName, List<string> binds) {
			_container.ContainerName = containerName;
			_container.Binds = binds;

			return this;
		}

		public IContainerBuilderService AddImage(string fromImage, string tag) {
			_container.FromImage = fromImage;
			_container.Tag = tag;

			return this;
		}

		public IContainerBuilderService PerformAuth(string registryUsername, string registryEmail, string registryPassword, string? registryAddress = null) {
			_container.RegistryAddress = registryAddress;
			_container.RegistryUsername = registryUsername;
			_container.RegistryEmail = registryEmail;
			_container.RegistryPassword = registryPassword;

			return this;
		}

		public IContainerBuilderService FromPipeline(Pipeline pipeline) {
			_container.Pipeline = pipeline;

			return this;
		}

		public async Task<ContainerBuilderResponse> Build() {
			await _client.Images.CreateImageAsync(
				new ImagesCreateParameters() {
					FromImage = _container.FromImage,
					Tag = _container.Tag
				},
				new AuthConfig() {
					Email = _container.RegistryEmail,
					Username = _container.RegistryUsername,
					Password = _container.RegistryPassword
				},
				new Progress<JSONMessage>()
			);

			var container = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
				Image = $"{_container.FromImage}:{_container.Tag}",
				Name = _container.ContainerName,
				Tty = true,
				AttachStdout = true,
				AttachStderr = true,
				AttachStdin = true,
				HostConfig = new HostConfig {
					Privileged = true,
					Binds = _container.Binds
				}
			});

			string containerId = container.ID[..12];
			await _client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());

			await GetSSHKey(containerId);
			await GenerateBashScript(containerId);
			var response = await ExecutePipelineScripts(containerId);

			await _client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
			await _client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());

			return response;
		}

		private async Task GetSSHKey(string containerId) {
			if (_container.Pipeline.DeployKey is null) {
				throw new ArgumentNullException(nameof(_container.Pipeline.DeployKey));
			}

			var generateSSHKeyCreateResponse = await _client.Exec.ExecCreateContainerAsync(containerId, new ContainerExecCreateParameters {
				Cmd = new List<string> {
					"/bin/bash",
					"-c",
					$"mkdir /root/.ssh; echo '{Encoding.UTF8.GetString(Convert.FromBase64String(_container.Pipeline.DeployKey)).Replace("\r\n", "\n").Replace("\r", "\n")}' >> /root/.ssh/id_ed25519"
				},
				AttachStdin = true,
				Tty = true
			}, default);

			await _client.Exec.StartAndAttachContainerExecAsync(generateSSHKeyCreateResponse.ID, false, default);
		}

		private async Task GenerateBashScript(string containerId) {
			if (_container.Pipeline.PipelineInstructions is null || !_container.Pipeline.PipelineInstructions.Any()) {
				throw new ArgumentNullException(nameof(_container.Pipeline.PipelineInstructions));
			}

			foreach (var instruction in _container.Pipeline.PipelineInstructions) {
				string? instructionScript = string.Join("\n", instruction.Script);

				if (instruction.PipelineInstructionInputs is not null) {
					instructionScript = ReplaceVariables(instruction.PipelineInstructionInputs.ToList(), instructionScript);
				}

				var generateScriptCreateResponse = await _client.Exec.ExecCreateContainerAsync(containerId, new ContainerExecCreateParameters {
					Cmd = new List<string> {
						"/bin/bash",
						"-c",
						$"mkdir scripts; echo '{instructionScript?.Replace("\r\n", "\n").Replace("\r", "\n")}' >> /scripts/script-{instruction.Id}.sh"
					},
					AttachStdin = true,
					Tty = true
				}, default);

				await _client.Exec.StartAndAttachContainerExecAsync(generateScriptCreateResponse.ID, false, default);
			}
		}

		private static string ReplaceVariables(List<PipelineInstructionInput> inputs, string script) {
			string pattern = @"\${([^}]+)}";
			Match match = Regex.Match(script, pattern);

			if (match.Success) {
				string key = match.Groups[1].Value;
				string replacement = inputs.Find(x => x.ConnectorFunctionInput.Replace == key)?.ReplaceValue ?? string.Empty;
				script = Regex.Replace(script, pattern, replacement);
			}

			return script;
		}

		private async Task<ContainerBuilderResponse> ExecutePipelineScripts(string containerId) {
			var response = new ContainerBuilderResponse();

			if (_container.Pipeline.PipelineInstructions is null || !_container.Pipeline.PipelineInstructions.Any()) {
				throw new ArgumentNullException(nameof(_container.Pipeline.PipelineInstructions));
			}

			foreach (var instruction in _container.Pipeline.PipelineInstructions) {
				var runScriptcreateResponse = await _client.Exec.ExecCreateContainerAsync(containerId, new ContainerExecCreateParameters {
					Cmd = new List<string> { "/bin/bash", "-c", $"cd /src; bash /scripts/script-{instruction.Id}.sh" },
					Detach = false,
					Tty = false,
					AttachStdout = true,
					AttachStderr = true,
					AttachStdin = true
				}, default);
				var stream = await _client.Exec.StartAndAttachContainerExecAsync(runScriptcreateResponse.ID, false, default);
				var inspectContainer = await _client.Exec.InspectContainerExecAsync(runScriptcreateResponse.ID);
				var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

				response.ExitCode = inspectContainer.ExitCode;
				response.Stdout += $"{stdout}\n{stderr}\n";

				if (inspectContainer.ExitCode != 0) {
					response.InstructionWithError = instruction.Id;
					break;
				}
			}

			return response;
		}
	}
}
