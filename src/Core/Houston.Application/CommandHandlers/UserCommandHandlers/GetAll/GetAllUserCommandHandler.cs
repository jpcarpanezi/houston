namespace Houston.Application.CommandHandlers.UserCommandHandlers.GetAll {
	public class GetAllUserCommandHandler : IRequestHandler<GetAllUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllUserCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllUserCommand request, CancellationToken cancellationToken) {
			var count = await _unitOfWork.UserRepository.Count();
			var users = await _unitOfWork.UserRepository.GetAll(request.PageSize, request.PageIndex);

			return ResultCommand.Paginated<User, UserViewModel>(users, request.PageSize, request.PageIndex, count);
		}
	}
}
