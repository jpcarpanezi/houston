using FluentValidation;
using Houston.Core.Commands.AuthCommands;

namespace Houston.API.Validators.AuthValidators {
	public class GeneralSignInCommandValidator : AbstractValidator<GeneralSignInCommand> {
		public GeneralSignInCommandValidator() {
			RuleFor(x => x.Email)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.Password)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(64).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
