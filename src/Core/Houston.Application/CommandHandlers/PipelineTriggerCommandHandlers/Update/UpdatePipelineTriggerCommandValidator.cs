namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public class UpdatePipelineTriggerCommandValidator : AbstractValidator<UpdatePipelineTriggerCommand> {
		public UpdatePipelineTriggerCommandValidator() {
			RuleFor(x => x.SourceGit)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Matches("^git@github\\.com:.+\\.git$").WithMessage(ValidatorsModelErrorMessages.URL)
				.MaximumLength(6000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Events).SetValidator(new UpdatePipelineTriggerEventsValidator());
		}
	}
}
