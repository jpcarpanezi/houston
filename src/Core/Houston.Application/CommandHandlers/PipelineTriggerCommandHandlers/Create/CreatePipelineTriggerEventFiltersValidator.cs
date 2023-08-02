namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public class CreatePipelineTriggerEventFiltersValidator : AbstractValidator<CreatePipelineTriggerEventFilters> {
		public CreatePipelineTriggerEventFiltersValidator() {
			RuleForEach(x => x.FilterValues)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
