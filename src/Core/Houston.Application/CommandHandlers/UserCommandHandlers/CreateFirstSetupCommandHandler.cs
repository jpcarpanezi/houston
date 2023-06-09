﻿using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Entities.Redis;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace Houston.Application.CommandHandlers.UserCommandHandlers {
	public class CreateFirstSetupCommandHandler : IRequestHandler<CreateFirstSetupCommand, ResultCommand<User>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;

		private const string ConfigurationKey = "configurations";
		private const string DefaultOs = "debian";
		private const string DefaultOsVersion = "11.6";

		public CreateFirstSetupCommandHandler(IUnitOfWork unitOfWork, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<ResultCommand<User>> Handle(CreateFirstSetupCommand request, CancellationToken cancellationToken) {
			var configurations = await _cache.GetStringAsync(ConfigurationKey);
			if (configurations is not null) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "The system has already been set up and configured.", "alreadyConfigured", null);
			}

			var anyUser = await _unitOfWork.UserRepository.AnyUser();
			if (anyUser) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "A user has already been registered in the system.", "userAlreadyRegistered", null);
			}

			var userId = Guid.NewGuid();
			var user = new User {
				Id = userId,
				Name = request.UserName,
				Email = request.UserEmail,
				Password = PasswordService.HashPassword(request.UserPassword),
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				Active = true,
				CreatedBy = userId,
				CreationDate = DateTime.MinValue,
				UpdatedBy = userId,
				LastUpdate = DateTime.UtcNow
			};
			
			_unitOfWork.UserRepository.Add(user);
			await _unitOfWork.Commit();

			var systemConfiguration = new SystemConfiguration(request.RegistryAddress, request.RegistryEmail, request.RegistryUsername, request.RegistryPassword, DefaultOs, DefaultOsVersion, false);
			await _cache.SetStringAsync("configurations", JsonSerializer.Serialize(systemConfiguration));

			return new ResultCommand<User>(HttpStatusCode.Created, null, null, user);
		}
	}
}
