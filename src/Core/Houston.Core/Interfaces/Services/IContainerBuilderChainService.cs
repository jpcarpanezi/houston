namespace Houston.Core.Interfaces.Services {
	public interface IContainerBuilderChainService {
		IContainerBuilderChainService Next { get; set; }

		Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters);
	}
}
