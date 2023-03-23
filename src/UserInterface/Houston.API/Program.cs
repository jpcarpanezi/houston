using Autofac.Extensions.DependencyInjection;
using Houston.API.Setups;
using Houston.Application.CommandHandlers.ConnectorCommandHandlers;
using Houston.Core.Converters;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Infrastructure.Context;
using Houston.Infrastructure.Repository;
using Houston.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllers(opts => opts.Filters.Add(new ProducesAttribute("application/json"))).AddJsonOptions(opts => {
	opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	opts.JsonSerializerOptions.Converters.Add(new ObjectIdConverter());
});
builder.Services.AddBearerAuthentication(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.SwaggerDoc("v1", new OpenApiInfo {
		Version = "v1",
		Title = "Houston CI",
		Description = "An easy CI pipeline creator using scratch concepts.",
	});

	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddAutoMapper(typeof(MapProfileSetup));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateConnectorCommandHandler>());
builder.Services.AddStackExchangeRedisCache(options => {
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
	options.InstanceName = "houston-";
});
builder.Services.AddEventBus(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserClaimsService, UserClaimsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Services.ConfigureEventBus();

app.Run();
