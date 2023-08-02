namespace Houston.API.Options {
	public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions> {
		private readonly IApiVersionDescriptionProvider _provider;

		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) {
			_provider = provider;
		}

		public void Configure(string? name, SwaggerGenOptions options) {
			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

			foreach (var description in _provider.ApiVersionDescriptions) {
				options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
			}
		}

		public void Configure(SwaggerGenOptions options) {
			Configure(options);
		}

		private static OpenApiInfo CreateVersionInfo(ApiVersionDescription desc) {
			var info = new OpenApiInfo() {
				Title = "Houston CI",
				Description = "An easy CI pipeline creator using scratch concepts.",
				Version = desc.ApiVersion.ToString()
			};

			if (desc.IsDeprecated) {
				info.Description += "<br><br><strong>This API version has been deprecated. Please use one of the new APIs available from the explorer.</strong>";
			}

			return info;
		}
	}
}
