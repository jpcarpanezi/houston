namespace Houston.Application.CommandHandlers.UserCommandHandlers.UpdateFirstAccess {
	public class UpdateFirstAccessPasswordCommandHandler : IRequestHandler<UpdateFirstAccessPasswordCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;

		public UpdateFirstAccessPasswordCommandHandler(IUnitOfWork unitOfWork, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<IResultCommand> Handle(UpdateFirstAccessPasswordCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.UserRepository.FindByEmail(request.Email);
			if (user is null || !user.FirstAccess || !user.Active) {
				return ResultCommand.Forbidden("Invalid token.", "invalidToken");
			}

			var token = await _cache.GetStringAsync(user.Id.ToString(), cancellationToken);
			if (token is null || token != request.Token) {
				return ResultCommand.Forbidden("Invalid token.", "invalidToken");
			}

			if (PasswordService.VerifyHashedPassword(request.Password, user.Password)) {
				return ResultCommand.BadRequest("The new password cannot be the same as previous one.", "passwordEqualToTemp");
			}

			user.Password = PasswordService.HashPassword(request.Password);
			user.FirstAccess = false;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = user.Id;

			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.Commit();

			await _cache.RemoveAsync(user.Id.ToString(), cancellationToken);

			return ResultCommand.NoContent();
		}
	}
}