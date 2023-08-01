namespace Houston.Application.CommandHandlers.UserCommandHandlers.CreateSetup {
	public class CreateFirstSetupCommandHandler : IRequestHandler<CreateFirstSetupCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;

		private const string ConfigurationKey = "configurations";
		private const string DefaultOs = "debian";
		private const string DefaultOsVersion = "11.6";

		public CreateFirstSetupCommandHandler(IUnitOfWork unitOfWork, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<IResultCommand> Handle(CreateFirstSetupCommand request, CancellationToken cancellationToken) {
			var configurations = await _cache.GetStringAsync(ConfigurationKey);
			if (configurations is not null) {
				return ResultCommand.Forbidden("The system has already been set up and configured.", "alreadyConfigured");
			}

			var anyUser = await _unitOfWork.UserRepository.AnyUser();
			if (anyUser) {
				return ResultCommand.Forbidden("A user has already been registered in the system.", "userAlreadyRegistered");
			}

			var userId = Guid.NewGuid();
			var user = new User {
				Id = userId,
				Name = request.UserName,
				Email = request.UserEmail,
				Password = PasswordService.HashPassword(request.UserPassword),
				Role = UserRoleEnum.Admin,
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
			await _cache.SetStringAsync("configurations", JsonSerializer.Serialize(systemConfiguration), cancellationToken);

			return ResultCommand.Created<User, UserViewModel>(user);
		}
	}
}
