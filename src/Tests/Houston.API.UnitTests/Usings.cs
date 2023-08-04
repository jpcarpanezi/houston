global using NUnit.Framework;
global using FluentAssertions;
global using AutoFixture;
global using Moq;

global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Options;

global using System.Net;
global using System.Text.Json;

global using EventBus.EventBus.Abstractions;
global using EventBus.EventBus.Events;

global using Houston.Core.Interfaces.Repository;
global using Houston.Core.Interfaces.Services;
global using Houston.Core.Entities.Postgres;
global using Houston.Core.Entities.Redis;

global using Houston.Application.Results;
global using Houston.Application.Security;
global using Houston.Application.ViewModel;
global using Houston.Application.ViewModel.ConnectorViewModels;
global using Houston.Application.ViewModel.ConnectorFunctionViewModels;
global using Houston.Application.ViewModel.PipelineInstructionViewModels;
global using Houston.Application.ViewModel.PipelineLogViewModels;
global using Houston.Application.ViewModel.PipelineViewModels;
global using Houston.Application.ViewModel.PipelineTriggerViewModels;