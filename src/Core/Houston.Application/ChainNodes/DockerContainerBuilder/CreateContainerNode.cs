using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using System.ComponentModel;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreateContainerNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public CreateContainerNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			var container = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
				Image = $"{parameters.FromImage}:{parameters.Tag}",
				Name = parameters.ContainerName,
				Tty = true,
				AttachStdout = true,
				AttachStderr = true,
				AttachStdin = true,
				HostConfig = new HostConfig {
					Privileged = true,
					Binds = parameters.Binds
				}
			});

			parameters.ContainerId = container.ID[..12];
			await _client.Containers.StartContainerAsync(parameters.ContainerId, new ContainerStartParameters());

			return await Next.Handler(solicitation, parameters);
		}
	}
}
