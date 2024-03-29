var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(builder.Configuration)
					.CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.ConfigureCors();

builder.Services.AddControllers(ExtensionOptions.ConfigureControllers)
				.AddJsonOptions(ExtensionOptions.ConfigureJson);

builder.Services.AddHttpContextAccessor();

builder.Services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.Load("Houston.Application"));

builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddApiVersioning(ExtensionOptions.ConfigureApiVersioning);

builder.Services.AddVersionedApiExplorer(ExtensionOptions.ConfigureApiVersioningExplorer);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddMassTransit(x => ExtensionOptions.ConfigureMassTransit(x, builder.Configuration));

builder.Services.AddPostgres(builder.Configuration, builder.Environment);

builder.Services.AddRepositories();

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddMediatR(ExtensionOptions.ConfigureMediatR)
				.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviorFilter<,>));


var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
	app.UseMigrations();
}

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI(opts => ExtensionOptions.ConfigureSwaggerUI(opts, app));
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();