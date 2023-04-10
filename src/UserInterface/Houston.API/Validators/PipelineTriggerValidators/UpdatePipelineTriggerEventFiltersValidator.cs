using FluentValidation;
using static Houston.Core.Commands.PipelineTriggerCommands.UpdatePipelineTriggerCommand;

namespace Houston.API.Validators.PipelineTriggerValidators {
	public class UpdatePipelineTriggerEventFiltersValidator : AbstractValidator<UpdatePipelineTriggerEventFilters> {
		public UpdatePipelineTriggerEventFiltersValidator() {
			RuleForEach(x => x.FilterValues)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
