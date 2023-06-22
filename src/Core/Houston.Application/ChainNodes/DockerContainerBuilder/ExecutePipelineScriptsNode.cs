using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using Microsoft.Extensions.Logging;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class ExecutePipelineScriptsNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<ExecutePipelineScriptsNode> _logger;

		public ExecutePipelineScriptsNode(IContainerBuilderChainService next, IDockerClient client, ILogger<ExecutePipelineScriptsNode> logger) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			_logger.LogInformation("Executing pipeline scripts for container {ContainerId}", parameters.ContainerId);

			if (!parameters.PipelineInstructions.Any()) {
				_logger.LogWarning("No pipeline instructions found for container {ContainerId}", parameters.ContainerId);

				solicitation.ExitCode = -1;
				solicitation.Stdout = "Could not find instructions to run the pipeline.";
				solicitation.InstructionWithError = null;

				return await Next.Handler(solicitation, parameters);
			}

			foreach (var instruction in parameters.PipelineInstructions.Select(x => x.Id)) {
				_logger.LogInformation("Starting execution of script for container: {ContainerId}, instruction: {InstructionId}", parameters.ContainerId, instruction);

				var runScriptcreateResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
					Cmd = new List<string> { "/bin/bash", "-c", $"cd /src; bash /scripts/script-{instruction}.sh" },
					Detach = false,
					Tty = false,
					AttachStdout = true,
					AttachStderr = true,
					AttachStdin = true
				}, default);

				_logger.LogInformation("Executing script for container: {ContainerId}, instruction: {InstructionId}", parameters.ContainerId, instruction);

				var stream = await _client.Exec.StartAndAttachContainerExecAsync(runScriptcreateResponse.ID, false, default);
				var inspectContainer = await _client.Exec.InspectContainerExecAsync(runScriptcreateResponse.ID);
				var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

				solicitation.ExitCode = inspectContainer.ExitCode;
				solicitation.Stdout += $"{stdout}\n{stderr}\n";

				if (inspectContainer.ExitCode != 0) {
					solicitation.InstructionWithError = instruction;
					break;
				}
				_logger.LogInformation("Script executed for container: {ContainerId}, instruction: {InstructionId}", parameters.ContainerId, instruction);
			}

			_logger.LogInformation("Pipeline scripts executed for container {ContainerId}", parameters.ContainerId);

			return await Next.Handler(solicitation, parameters);
		}
	}
}
