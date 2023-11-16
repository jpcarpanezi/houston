﻿global using System.Net;
global using System.Globalization;
global using System.Text.Json;
global using System.Reflection;
global using System.Security.Cryptography;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text.RegularExpressions;
global using System.Collections;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Options;

global using MediatR;
global using MediatR.Pipeline;
global using MassTransit;
global using Serilog;
global using AutoMapper;
global using FluentValidation;
global using Docker.DotNet;
global using Docker.DotNet.Models;
global using Grpc.Net.Client;
global using Grpc.Core;
global using SharpCompress.Common;
global using SharpCompress.Writers;
global using YamlDotNet.Serialization;
global using YamlDotNet.Serialization.NamingConventions;

global using Houston.Infrastructure.Services;

global using Houston.Application.Results;
global using Houston.Application.Security;
global using Houston.Application.ViewModel.WorkerViewModels;
global using Houston.Application.ViewModel;
global using Houston.Application.ViewModel.ConnectorViewModels;
global using Houston.Application.ViewModel.ConnectorFunctionViewModels;
global using Houston.Application.ViewModel.PipelineLogViewModels;
global using Houston.Application.ViewModel.PipelineViewModels;
global using Houston.Application.ViewModel.PipelineTriggerViewModels;
global using Houston.Application.ViewModel.UserViewModels;
global using Houston.Application.CommandHandlers.WorkerCommandHandlers.RunPipeline;

global using Houston.Core.Interfaces.Repository;
global using Houston.Core.Interfaces.Results;
global using Houston.Core.Entities.Postgres;
global using Houston.Core.Interfaces.Services;
global using Houston.Core.Messages;
global using Houston.Core.Enums;
global using Houston.Core.Services;
global using Houston.Core.Entities.Redis;
global using Houston.Core.Exceptions;
global using Houston.Core.Models;
global using Houston.Core.Commands;
global using Houston.Core.Models.Options;
global using Houston.Core.Proto.Houston.v1;
global using Houston.Core.Models.YamlSpecs;