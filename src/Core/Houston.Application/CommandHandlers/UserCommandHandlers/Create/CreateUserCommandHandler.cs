namespace Houston.Application.CommandHandlers.UserCommandHandlers.Create {
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			var checkUserExists = await _unitOfWork.UserRepository.FindByEmail(request.Email);
			if (checkUserExists is not null) {
				return ResultCommand.Conflict("A user with this email address already exists in the system.", "userAlreadyExists");
			}

			var user = new User {
				Id = Guid.NewGuid(),
				Name = request.Name,
				Email = request.Email,
				Password = PasswordService.HashPassword(request.TempPassword),
				Role = request.UserRole,
				FirstAccess = true,
				Active = true,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.UserRepository.Add(user);
			await _unitOfWork.Commit();

			return ResultCommand.Created<User, UserViewModel>(user);
		}
	}
}
