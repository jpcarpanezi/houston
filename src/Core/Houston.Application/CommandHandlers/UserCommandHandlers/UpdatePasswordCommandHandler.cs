using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.UserCommandHandlers {
	public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ResultCommand<User>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePasswordCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<User>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken) {
			if (request.UserId is not null && !_claims.Roles.Contains(Core.Enums.UserRoleEnum.Admin)) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "Only administators are allowed to change the password of other users.", null);
			}

			Guid userId = request.UserId is null ? _claims.Id : (Guid)request.UserId;
			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user is null) {
				return new ResultCommand<User>(HttpStatusCode.NotFound, "The requested user could not be found.", null);
			}

			if (!user.Active) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "This user account is inactive.", null);
			}

			if (request.UserId is null && request.OldPassword is not null && !PasswordService.VerifyHashedPassword(request.OldPassword, user.Password)) {
				return new ResultCommand<User>(HttpStatusCode.BadRequest, "The old password provided is incorrect", null);
			}

			user.Password = PasswordService.HashPassword(request.NewPassword);
			user.FirstAccess = request.UserId is not null;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = _claims.Id;
			
			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.Commit();

			return new ResultCommand<User>(HttpStatusCode.NoContent, null, null);
		}
	}
}
