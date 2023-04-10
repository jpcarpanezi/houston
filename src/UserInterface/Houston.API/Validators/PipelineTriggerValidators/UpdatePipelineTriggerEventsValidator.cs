using FluentValidation;
using static Houston.Core.Commands.PipelineTriggerCommands.UpdatePipelineTriggerCommand;

namespace Houston.API.Validators.PipelineTriggerValidators {
	public class UpdatePipelineTriggerEventsValidator : AbstractValidator<UpdatePipelineTriggerEvents> {
		public UpdatePipelineTriggerEventsValidator() {
			RuleForEach(x => x.EventFilters).SetValidator(new UpdatePipelineTriggerEventFiltersValidator());
		}
	}
}
