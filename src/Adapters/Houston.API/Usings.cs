global using System.Net;
global using System.Reflection;
global using System.Text.Json.Serialization;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Mvc.Filters;

global using Npgsql;
global using Autofac.Extensions.DependencyInjection;
global using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using MediatR;
global using Autofac;

global using EventBus.EventBus;
global using EventBus.EventBus.Abstractions;
global using EventBus.EventBusRabbitMQ;
global using RabbitMQ.Client;

global using Houston.Application.ViewModel;
global using Houston.Application.Security;

global using Houston.Core.Interfaces.Repository;
global using Houston.Core.Interfaces.Services;

global using Houston.Infrastructure.Repository;
global using Houston.Infrastructure.Services;
global using Houston.Infrastructure.Context;

global using Houston.API.Filters;
global using Houston.API.Setups;