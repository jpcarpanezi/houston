using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Houston.API.Filters;
using Houston.API.Setups;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Infrastructure.Repository;
using Houston.Infrastructure.Services;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddCors(options => {
	options.AddDefaultPolicy(build => {
		build
			.WithOrigins(builder.Configuration.GetSection("CorsSettings:Origins").Get<string[]>())
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials();
	});
});
builder.Services.AddControllers(opts => {
	opts.Filters.Add(new ProducesAttribute("application/json"));
	opts.Filters.Add(new ForeignKeyExceptionFilter());
}).AddJsonOptions(opts => {
	opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationClientsideAdapters();
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
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Houston.Application")));
builder.Services.AddStackExchangeRedisCache(options => {
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
	options.InstanceName = "houston-";
});
builder.Services.AddEventBus(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddPostgres(builder.Configuration, builder.Environment);
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserClaimsService, UserClaimsService>();

var app = builder.Build();

app.UseMigrations();
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Services.ConfigureEventBus();

app.Run();
