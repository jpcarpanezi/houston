namespace Houston.Application.CommandHandlers.UserCommandHandlers.Get {
	public class GetUserCommandHandler : IRequestHandler<GetUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetUserCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetUserCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
			if (user is null) {
				return ResultCommand.NotFound("The requested user could not be found.", "userNotFound");
			}

			return ResultCommand.Ok<User, UserViewModel>(user);
		}
	}
}
