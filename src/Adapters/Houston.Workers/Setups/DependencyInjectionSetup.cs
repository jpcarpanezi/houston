using Docker.DotNet;
using Houston.Application.ChainNodes.DockerContainerBuilder;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Infrastructure.Repository;
using Houston.Infrastructure.Services;

namespace Houston.Workers.Setups {
	public static class DependencyInjectionSetup {
		public static IServiceCollection AddDependencyInjection(this IServiceCollection services) {
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IContainerBuilderParametersService, DockerContainerBuilderParametersService>();
			services.AddSingleton<IDockerClient>(sp => {
				var dockerSock = "unix:///var/run/docker.sock";
				return new DockerClientConfiguration(new Uri(dockerSock)).CreateClient();
			});

			services.Chain<IContainerBuilderChainService>()
					.Add<CreateContainerImageNode>()
					.Add<CreateContainerNode>()
					.Add<CreatePipelineDeployKeyNode>()
					.Add<CloneRepositoryNode>()
					.Add<CreatePipelineScriptsNode>()
					.Add<ExecutePipelineScriptsNode>()
					.Add<RemoveContainerNode>()
					.Configure();

			return services;
		}
	}
}
