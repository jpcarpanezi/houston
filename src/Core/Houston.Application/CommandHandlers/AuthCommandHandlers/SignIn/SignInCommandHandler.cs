namespace Houston.Application.CommandHandlers.AuthCommandHandlers.SignIn {
	public class SignInCommandHandler : IRequestHandler<SignInCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly SigningConfigurations _signingConfigurations;
		private readonly TokenConfigurations _tokenConfigurations;
		private readonly IDistributedCache _cache;

		public SignInCommandHandler(IUnitOfWork unitOfWork, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_signingConfigurations = signingConfigurations ?? throw new ArgumentNullException(nameof(signingConfigurations));
			_tokenConfigurations = tokenConfigurations ?? throw new ArgumentNullException(nameof(tokenConfigurations));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<IResultCommand> Handle(SignInCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.UserRepository.FindByEmail(request.Email);

			if (user is null || !PasswordService.VerifyHashedPassword(request.Password, user.Password)) {
				return ResultCommand.Forbidden("Invalid username or password.", "invalidCredentials");
			}

			if (!user.Active) {
				return ResultCommand.Unauthorized("The account has been deactivated.", "userInactive");
			}

			if (user.FirstAccess) {
				var passwordToken = await _cache.GetStringAsync(user.Id.ToString(), cancellationToken);

				if (passwordToken is null) {
					passwordToken = Guid.NewGuid().ToString("N");
					DistributedCacheEntryOptions cacheOptions = new();
					cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
					await _cache.SetStringAsync(user.Id.ToString(), passwordToken, cacheOptions, cancellationToken);
				}

				var body = new FirstAccessViewModel(passwordToken, $"/api/User/firstAccess/{passwordToken}", "First login. Need to create a new password to continue.", "firstAccess");
				return ResultCommand.TemporaryRedirect(body);
			}

			return ResultCommand.Ok<BearerTokenViewModel, BearerTokenViewModel>(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}
	}
}
