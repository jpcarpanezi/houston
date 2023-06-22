using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Entities.Postgres;
using Houston.Core.Exceptions;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreatePipelineScriptsNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<CreatePipelineScriptsNode> _logger;

		public CreatePipelineScriptsNode(IContainerBuilderChainService next, IDockerClient client, ILogger<CreatePipelineScriptsNode> logger) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			_logger.LogInformation("Creating pipeline scripts for container {ContainerId}", parameters.ContainerId);

			foreach (var instruction in parameters.PipelineInstructions) {
				string? instructionScript = string.Join("\n", instruction.Script);

				if (instruction.PipelineInstructionInputs is not null) {
					instructionScript = ReplaceVariables(instruction.PipelineInstructionInputs.ToList(), instructionScript);
				}

				_logger.LogInformation("Generating script create response for container: {ContainerId}, instruction: {InstructionId}", parameters.ContainerId, instruction.Id);

				var generateScriptCreateResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
					Cmd = new List<string> {
						"/bin/bash",
						"-c",
						$"mkdir scripts; echo '{instructionScript?.Replace("\r\n", "\n").Replace("\r", "\n")}' >> /scripts/script-{instruction.Id}.sh"
					},
					AttachStdin = true,
					Tty = true
				}, default);

				_logger.LogInformation("Started and attached container exec for script generate response for container: {ContainerId}, instruction: {InstructionId}", parameters.ContainerId, instruction.Id);

				var stream = await _client.Exec.StartAndAttachContainerExecAsync(generateScriptCreateResponse.ID, false, default);
				var inspectContainer = await _client.Exec.InspectContainerExecAsync(generateScriptCreateResponse.ID);
				var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

				if (inspectContainer.ExitCode != 0)
					throw new ContainerBuilderException("An error occurred when creating the pipeline scripts.", $"{stdout}\n{stderr}\n");

				_logger.LogInformation("Finished container exec and inspect for script generate response for container: {ContainerId}, instruction: {InstructionId}", parameters.ContainerId, instruction.Id);
			}


			_logger.LogInformation("Finished pipeline script execution for container {ContainerId}", parameters.ContainerId);

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
