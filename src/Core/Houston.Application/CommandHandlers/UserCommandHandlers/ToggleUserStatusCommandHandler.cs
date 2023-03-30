using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.UserCommandHandlers {
	public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, ResultCommand<User>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public ToggleUserStatusCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<User>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken) {
			if (request.UserId == _claims.Id) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "selfUpdateNotAllowed", null);
			}

			var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
			if (user is null) {
				return new ResultCommand<User>(HttpStatusCode.NotFound, "userNotFound", null);
			}

			user.Active = !user.Active;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = _claims.Id;

			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.Commit();

			return new ResultCommand<User>(HttpStatusCode.NoContent, null, null);
		}
	}
}
