using Autofac.Extensions.DependencyInjection;
using Houston.API.Setups;
using Houston.Application.CommandHandlers.ConnectorCommandHandlers;
using Houston.Core.Converters;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;
using Houston.Infrastructure.Repository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllers().AddJsonOptions(opts => {
	opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	opts.JsonSerializerOptions.Converters.Add(new ObjectIdConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateConnectorCommandHandler>());
builder.Services.AddEventBus(builder.Configuration);
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Services.ConfigureEventBus();

app.Run();
