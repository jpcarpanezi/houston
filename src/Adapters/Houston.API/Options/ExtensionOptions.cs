namespace Houston.API.Options {
	public static class ExtensionOptions {
		public static void ConfigureMediatR(MediatRServiceConfiguration options) {
			options.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Houston.Application"));
		}

		public static void ConfigureSwaggerUI(SwaggerUIOptions options, WebApplication app) {
			var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

			foreach (var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse().Select(x => x.GroupName)) {
				options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json",
					groupName.ToUpperInvariant());
			}
		}

		public static void ConfigureControllers(MvcOptions options) {
			options.Filters.Add(new ProducesAttribute("application/json"));
			options.Filters.Add(new ForeignKeyExceptionFilter());
			options.Filters.Add(new ValidationExceptionFilter());
		}

		public static void ConfigureJson(JsonOptions options) {
			options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		}

		public static void ConfigureApiVersioning(ApiVersioningOptions options) {
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;
			options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
															new HeaderApiVersionReader("x-api-version"),
															new MediaTypeApiVersionReader("x-api-version"));
		}

		public static void ConfigureApiVersioningExplorer(ApiExplorerOptions options) {
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		}
	}
}