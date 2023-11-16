namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public class CreatePipelineTriggerCommandValidator : AbstractValidator<CreatePipelineTriggerCommand> {
		public CreatePipelineTriggerCommandValidator() {
			RuleFor(x => x.SourceGit)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Matches("^git@github\\.com:.+\\.git$").WithMessage(ValidatorsModelErrorMessages.URL)
				.MaximumLength(6000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Secret)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters);
		}
	}
}
