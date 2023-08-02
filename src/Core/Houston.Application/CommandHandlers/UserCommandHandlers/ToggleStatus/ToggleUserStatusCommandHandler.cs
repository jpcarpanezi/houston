namespace Houston.Application.CommandHandlers.UserCommandHandlers.ToggleStatus {
	public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public ToggleUserStatusCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken) {
			if (request.UserId == _claims.Id) {
				return ResultCommand.Forbidden("Self-updating is not allowed for this resource.", "selfUpdatingNotAllowed");
			}

			var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
			if (user is null) {
				return ResultCommand.NotFound("The requested user could not be found.", "userNotFound");
			}

			user.Active = !user.Active;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = _claims.Id;

			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
