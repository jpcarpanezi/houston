namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.ChangeSecret {
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
