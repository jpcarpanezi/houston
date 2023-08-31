namespace Houston.Workers.Extensions {
	public static class DependencyInjectionExtension {
		public static IServiceCollection AddDependencyInjections(this IServiceCollection services) {
			services.AddSingleton<IDockerClient>(sp => {
				string dockerSock = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";
				return new DockerClientConfiguration(new Uri(dockerSock)).CreateClient();
			});

			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CreateContainerImageBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CreateContainerBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CloneRepositoryBehavior<,>));

			return services;
		}
	}
}
