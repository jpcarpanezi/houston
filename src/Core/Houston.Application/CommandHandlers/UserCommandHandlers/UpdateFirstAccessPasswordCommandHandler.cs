using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

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

			var token = await _cache.GetStringAsync(user.Id.ToString());
			if (token is null) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidToken", null);
			}

			if (token != request.Token) {
				return new ResultCommand<User>(HttpStatusCode.Forbidden, "invalidToken", null);
			}

			if (!PasswordService.IsPasswordStrong(request.Password)) {
				return new ResultCommand<User>(HttpStatusCode.BadRequest, "passwordNotStrong", null);
			}

			if (PasswordService.VerifyHashedPassword(request.Password, user.Password)) {
				return new ResultCommand<User>(HttpStatusCode.BadRequest, "passwordEqualToTemp", null);
			}

			user.Password = PasswordService.HashPassword(request.Password);
			user.IsFirstAccess = false;
			user.LastUpdate = DateTime.UtcNow;
			user.UpdatedBy = user.Id;
			await _unitOfWork.UserRepository.ReplaceOneAsync(user);

			await _cache.RemoveAsync(user.Id.ToString());

			return new ResultCommand<User>(HttpStatusCode.NoContent, null, null);
		}
	}
}
