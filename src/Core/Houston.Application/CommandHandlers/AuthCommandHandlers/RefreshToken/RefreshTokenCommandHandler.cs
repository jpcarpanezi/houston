namespace Houston.Application.CommandHandlers.AuthCommandHandlers.RefreshToken {
	public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly SigningConfigurations _signingConfigurations;
		private readonly TokenConfigurations _tokenConfigurations;
		private readonly IDistributedCache _cache;

		public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_signingConfigurations = signingConfigurations ?? throw new ArgumentNullException(nameof(signingConfigurations));
			_tokenConfigurations = tokenConfigurations ?? throw new ArgumentNullException(nameof(tokenConfigurations));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<IResultCommand> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
			if (string.IsNullOrWhiteSpace(request.Token)) {
				return ResultCommand.Forbidden("Invalid token.", "invalidToken");
			}

			string? redisToken = await _cache.GetStringAsync(request.Token, cancellationToken);
			if (redisToken is null) {
				return ResultCommand.Forbidden("Token expired.", "invalidToken");
			}

			RefreshTokenData? tokenData = JsonSerializer.Deserialize<RefreshTokenData>(redisToken);
			var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(tokenData!.UserId));
			if (user is null) {
				return ResultCommand.Forbidden("User not found.", "userNotFound");
			}

			if (!user.Active) {
				return ResultCommand.Forbidden("User inactive.", "userNotFound");
			}
			await _cache.RemoveAsync(request.Token, cancellationToken);

			return ResultCommand.Ok<BearerTokenViewModel, BearerTokenViewModel>(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}
	}
}
