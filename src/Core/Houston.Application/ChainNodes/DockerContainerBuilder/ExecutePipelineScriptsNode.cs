﻿using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class ExecutePipelineScriptsNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public ExecutePipelineScriptsNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			if (!parameters.PipelineInstructions.Any()) {
				solicitation.ExitCode = -1;
				solicitation.Stdout = "Could not find instructions to run the pipeline.";
				solicitation.InstructionWithError = null;

				return await Next.Handler(solicitation, parameters);
			}

			foreach (var instruction in parameters.PipelineInstructions.Select(x => x.Id)) {
				var runScriptcreateResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
					Cmd = new List<string> { "/bin/bash", "-c", $"cd /src; bash /scripts/script-{instruction}.sh" },
					Detach = false,
					Tty = false,
					AttachStdout = true,
					AttachStderr = true,
					AttachStdin = true
				}, default);
				var stream = await _client.Exec.StartAndAttachContainerExecAsync(runScriptcreateResponse.ID, false, default);
				var inspectContainer = await _client.Exec.InspectContainerExecAsync(runScriptcreateResponse.ID);
				var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

				solicitation.ExitCode = inspectContainer.ExitCode;
				solicitation.Stdout += $"{stdout}\n{stderr}\n";

				if (inspectContainer.ExitCode != 0) {
					solicitation.InstructionWithError = instruction;
					break;
				}
			}

			return await Next.Handler(solicitation, parameters);
		}
	}
}
