using FluentValidation;
using Houston.Core.Commands.UserCommands;

namespace Houston.API.Validators.UserValidators {
	public class UpdateFirstAccessPasswordCommandValidator : AbstractValidator<UpdateFirstAccessPasswordCommand> {
		public UpdateFirstAccessPasswordCommandValidator() {
			RuleFor(x => x.Email)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.Token)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(x => Guid.TryParseExact(x, "N", out _)).WithMessage("invalidToken");

			RuleFor(x => x.Password)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.MaximumLength(64).WithMessage(ValidatorsModelErrorMessages.MaxLength)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters);
		}
	}
}
