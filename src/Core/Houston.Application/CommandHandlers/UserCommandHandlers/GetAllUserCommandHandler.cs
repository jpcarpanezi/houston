using Houston.Core.Commands;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;

namespace Houston.Application.CommandHandlers.UserCommandHandlers {
	public class GetAllUserCommandHandler : IRequestHandler<GetAllUserCommand, PaginatedResultCommand<User>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllUserCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<PaginatedResultCommand<User>> Handle(GetAllUserCommand request, CancellationToken cancellationToken) {
			var count = await _unitOfWork.UserRepository.Count();
			var users = await _unitOfWork.UserRepository.GetAll(request.PageSize, request.PageIndex);

			return new PaginatedResultCommand<User>(users, request.PageSize, request.PageIndex, count);
		}
	}
}
