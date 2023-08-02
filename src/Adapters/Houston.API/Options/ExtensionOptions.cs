namespace Houston.API.Options {
	public static class ExtensionOptions {
		public static void ConfigureMediatR(MediatRServiceConfiguration options) {
			options.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Houston.Application"));
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
	}
}