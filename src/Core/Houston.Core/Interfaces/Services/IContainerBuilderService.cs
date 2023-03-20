using Houston.Core.Entities.MongoDB;
using Houston.Core.Models;

namespace Houston.Core.Interfaces.Services {
	public interface IContainerBuilderService {
		IContainerBuilderService PerformAuth(string registryUsername, string registryEmail, string registryPassword, string? registryAddress = null);

		IContainerBuilderService AddImage(string fromImage, string tag);

		IContainerBuilderService AddContainer(string containerName, List<string> binds);

		IContainerBuilderService FromPipeline(Pipeline pipeline);

		Task<ContainerBuilderResponse> Build();
	}
}
