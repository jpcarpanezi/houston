namespace Houston.Application.CommandHandlers.UserCommandHandlers.Create {
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
		public CreateUserCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(240).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Email)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.TempPassword)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters);

			RuleFor(x => x.UserRole)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null)
				.IsInEnum().WithMessage("invalidRole");
		}
	}
}
