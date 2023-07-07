using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.UserCommandHandlers {
	public class GetUserCommandHandler : IRequestHandler<GetUserCommand, ResultCommand<User>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetUserCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<User>> Handle(GetUserCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
			if (user is null) {
				return new ResultCommand<User>(HttpStatusCode.NotFound, "The requested user could not be found.", "userNotFound", null);
			}

			return new ResultCommand<User>(HttpStatusCode.OK, null, null, user);
		}
	}
}
