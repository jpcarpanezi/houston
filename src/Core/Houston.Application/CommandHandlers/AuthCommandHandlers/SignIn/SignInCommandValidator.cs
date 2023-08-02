namespace Houston.Application.CommandHandlers.AuthCommandHandlers.SignIn {
	public class SignInCommandValidator : AbstractValidator<SignInCommand> {
		public SignInCommandValidator() {
			RuleFor(x => x.Email)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.Password)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(64).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
