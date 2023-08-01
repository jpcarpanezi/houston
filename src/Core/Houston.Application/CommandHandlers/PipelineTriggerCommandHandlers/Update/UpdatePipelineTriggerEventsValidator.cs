namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public class UpdatePipelineTriggerEventsValidator : AbstractValidator<UpdatePipelineTriggerEvents> {
		public UpdatePipelineTriggerEventsValidator() {
			RuleForEach(x => x.EventFilters).SetValidator(new UpdatePipelineTriggerEventFiltersValidator());
		}
	}
}
