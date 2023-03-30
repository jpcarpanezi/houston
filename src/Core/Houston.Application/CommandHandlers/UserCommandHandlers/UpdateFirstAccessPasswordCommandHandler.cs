using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace Houston.Application.CommandHandlers.UserCommandHandlers {
	public class UpdateFirstAccessPasswordCommandHandler : IRequestHandler<UpdateFirstAccessPasswordCommand, ResultCommand<User>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;

		public UpdateFirstAccessPasswordCommandHandler(IUnitOfWork unitOfWork, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<ResultCommand<User>> Handle(UpdateFirstAccessPasswordCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.UserRepository.FindByEmail(request.Email);
			if (user is null) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidToken", null);
			}

			if (!user.FirstAccess || !user.Active) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidToken", null);
			}

			var token = await _cache.GetStringAsync(user.Id.ToString());
			if (token is null) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidToken", null);
			}

			if (token != request.Token) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidToken", null);
			}

			if (!PasswordService.IsPasswordStrong(request.Password)) {
				return new ResultCommand<User>(HttpStatusCode.BadRequest, "weakPassword", null);
			}

			if (PasswordService.VerifyHashedPassword(request.Password, user.Password)) {
				return new ResultCommand<User>(HttpStatusCode.BadRequest, "passwordEqualToTemp", null);
			}

			user.Password = PasswordService.HashPassword(request.Password);
			user.FirstAccess = false;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = user.Id;

			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.Commit();

			await _cache.RemoveAsync(user.Id.ToString());

			return new ResultCommand<User>(HttpStatusCode.NoContent, null, null);
		}
	}
}
