var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog();

builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.ConfigureCors();

builder.Services.AddControllers(ExtensionOptions.ConfigureControllers)
				.AddJsonOptions(ExtensionOptions.ConfigureJson);

builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddApiVersioning(ExtensionOptions.ConfigureApiVersioning);

builder.Services.AddVersionedApiExplorer(ExtensionOptions.ConfigureApiVersioningExplorer);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddValidatorsFromAssembly(typeof(Houston.Application.ValidatorsModelErrorMessages).Assembly);

builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddMediatR(ExtensionOptions.ConfigureMediatR)
				.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviorFilter<,>));

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddEventBus(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddPostgres(builder.Configuration, builder.Environment);

builder.Services.AddRepositories();


var app = builder.Build();

app.UseMigrations();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI(opts => ExtensionOptions.ConfigureSwaggerUI(opts, app));
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Services.ConfigureEventBus();

app.Run();