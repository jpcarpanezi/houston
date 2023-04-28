using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Entities.Postgres;
using Houston.Core.Exceptions;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreatePipelineScriptsNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public CreatePipelineScriptsNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			foreach (var instruction in parameters.PipelineInstructions) {
				string? instructionScript = string.Join("\n", instruction.Script);

				if (instruction.PipelineInstructionInputs is not null) {
					instructionScript = ReplaceVariables(instruction.PipelineInstructionInputs.ToList(), instructionScript);
				}

				var generateScriptCreateResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
					Cmd = new List<string> {
						"/bin/bash",
						"-c",
						$"mkdir scripts; echo '{instructionScript?.Replace("\r\n", "\n").Replace("\r", "\n")}' >> /scripts/script-{instruction.Id}.sh"
					},
					AttachStdin = true,
					Tty = true
				}, default);

				var stream = await _client.Exec.StartAndAttachContainerExecAsync(generateScriptCreateResponse.ID, false, default);
				var inspectContainer = await _client.Exec.InspectContainerExecAsync(generateScriptCreateResponse.ID);
				var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

				if (inspectContainer.ExitCode != 0)
					throw new ContainerBuilderException("An error occurred when creating the pipeline scripts.", $"{stdout}\n{stderr}\n");
			}

			return await Next.Handler(solicitation, parameters);
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
	}
}
