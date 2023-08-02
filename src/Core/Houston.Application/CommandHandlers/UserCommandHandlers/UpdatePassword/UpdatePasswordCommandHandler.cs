namespace Houston.Application.CommandHandlers.UserCommandHandlers.UpdatePassword {
	public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePasswordCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken) {
			if (request.UserId is not null && !_claims.Roles.Contains(UserRoleEnum.Admin)) {
				return ResultCommand.Forbidden("Only administators are allowed to change the password of other users.", "unauthorizedPasswordChange");
			}

			Guid userId = request.UserId is null ? _claims.Id : (Guid)request.UserId;
			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user is null) {
				return ResultCommand.NotFound("The requested user could not be found.", "userNotFound");
			}

			if (!user.Active) {
				return ResultCommand.Forbidden("This user account is inactive.", "inactiveUser");
			}

			if (request.UserId is null && request.OldPassword is not null && !PasswordService.VerifyHashedPassword(request.OldPassword, user.Password)) {
				return ResultCommand.BadRequest("The old password provided is incorrect.", "incorrectOldPassword");
			}

			user.Password = PasswordService.HashPassword(request.NewPassword);
			user.FirstAccess = request.UserId is not null;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = _claims.Id;

			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
