using FluentValidation;
using static Houston.Core.Commands.PipelineTriggerCommands.CreatePipelineTriggerCommand;

namespace Houston.API.Validators.PipelineTriggerValidators {
	public class CreatePipelineTriggerEventsValidator : AbstractValidator<CreatePipelineTriggerEvents> {
		public CreatePipelineTriggerEventsValidator() {
			RuleForEach(x => x.EventFilters).SetValidator(new CreatePipelineTriggerEventFiltersValidator());
		}
	}
}
