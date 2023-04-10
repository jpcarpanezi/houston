using FluentValidation;
using Houston.Core.Commands.PipelineTriggerCommands;

namespace Houston.API.Validators.PipelineTriggerValidators {
	public class ChangeSecretPipelineTriggerCommandValidator : AbstractValidator<ChangeSecretPipelineTriggerCommand> {
		public ChangeSecretPipelineTriggerCommandValidator() {
			RuleFor(x => x.NewSecret)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters);
		}
	}
}
