using FluentValidation;
using Houston.Core.Commands.UserCommands;

namespace Houston.API.Validators.UserValidators {
	public class CreateFirstSetupCommandValidator : AbstractValidator<CreateFirstSetupCommand> {
		public CreateFirstSetupCommandValidator() {
			RuleFor(x => x.RegistryAddress)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Matches(@"^(https?://)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*/?$").WithMessage(ValidatorsModelErrorMessages.URL)
				.MaximumLength(6000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.RegistryEmail)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.RegistryUsername)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.RegistryPassword)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters);

			RuleFor(x => x.UserName)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(240).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.UserEmail)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.UserPassword)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.MaximumLength(64).WithMessage(ValidatorsModelErrorMessages.MaxLength)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters);
		}
	}
}
