namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public class CreatePipelineTriggerEventsValidator : AbstractValidator<CreatePipelineTriggerEvents> {
		public CreatePipelineTriggerEventsValidator() {
			RuleForEach(x => x.EventFilters).SetValidator(new CreatePipelineTriggerEventFiltersValidator());
		}
	}
}
