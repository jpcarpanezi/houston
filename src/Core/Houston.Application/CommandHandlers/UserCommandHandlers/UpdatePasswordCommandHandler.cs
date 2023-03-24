using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Services;
using MediatR;
using MongoDB.Bson;
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
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "unauthorizedPasswordChange", null);
			}

			ObjectId userId = request.UserId is null ? _claims.Id : (ObjectId)request.UserId;
			var user = await _unitOfWork.UserRepository.FindByIdAsync(userId);
			if (user is null) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidUser", null);
			}

			if (request.UserId is null && request.OldPassword is not null && !PasswordService.VerifyHashedPassword(request.OldPassword, user.Password)) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "incorrectOldPassword", null);
			}

			if (!PasswordService.IsPasswordStrong(request.NewPassword)) {
				return new ResultCommand<User>(HttpStatusCode.BadRequest, "passwordNotStrong", null);
			}

			user.Password = PasswordService.HashPassword(request.NewPassword);
			user.IsFirstAccess = request.UserId is not null;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = _claims.Id;
			
			await _unitOfWork.UserRepository.ReplaceOneAsync(user);

			return new ResultCommand<User>(HttpStatusCode.NoContent, null, null);
		}
	}
}
