namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public class UpdatePipelineTriggerEventFiltersValidator : AbstractValidator<UpdatePipelineTriggerEventFilters> {
		public UpdatePipelineTriggerEventFiltersValidator() {
			RuleForEach(x => x.FilterValues)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
