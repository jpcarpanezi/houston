using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;

namespace Houston.Infrastructure.Services {
	public class DockerContainerBuilderParametersService : IContainerBuilderParametersService {
		private readonly ContainerBuilderParameters _container = new();

		public IContainerBuilderParametersService AddImage(string fromImage, string tag) {
			_container.FromImage = fromImage;
			_container.Tag = tag;

			return this;
		}

		public IContainerBuilderParametersService AddAuthentication(string registryUsername, string registryEmail, string registryPassword, string? registryAddress = null) {
			_container.RegistryAddress = registryAddress;
			_container.RegistryUsername = registryUsername;
			_container.RegistryEmail = registryEmail;
			_container.RegistryPassword = registryPassword;

			return this;
		}

		public IContainerBuilderParametersService AddContainerName(string containerName) {
			_container.ContainerName = containerName;

			return this;
		}

		public IContainerBuilderParametersService AddBind(string bind) {
			_container.Binds.Add(bind);

			return this;
		}

		public IContainerBuilderParametersService AddInstructions(List<PipelineInstruction> instructions) {
			_container.PipelineInstructions = instructions;
			
			return this;
		}

		public IContainerBuilderParametersService AddDeployKey(string deployKey) {
			_container.DeployKey = deployKey;

			return this;
		}

		public IContainerBuilderParametersService AddSourceGit(string sourceGit) {
			_container.SourceGit = sourceGit;

			return this;
		}

		public ContainerBuilderParameters Build() => _container;
	}
}
