using Houston.Core.Entities.Postgres;
using Houston.Core.Models;

namespace Houston.Core.Interfaces.Services {
	public interface IContainerBuilderParametersService {
		IContainerBuilderParametersService AddAuthentication(string registryUsername, string registryEmail, string registryPassword, string? registryAddress = null);

		IContainerBuilderParametersService AddImage(string fromImage, string tag);

		IContainerBuilderParametersService AddContainerName(string containerName);

		IContainerBuilderParametersService AddBind(string bind);

		IContainerBuilderParametersService AddInstructions(List<PipelineInstruction> instructions);

		IContainerBuilderParametersService AddDeployKey(string deployKey);

		IContainerBuilderParametersService AddSourceGit(string sourceGit);

		ContainerBuilderParameters Build();
	}
}
