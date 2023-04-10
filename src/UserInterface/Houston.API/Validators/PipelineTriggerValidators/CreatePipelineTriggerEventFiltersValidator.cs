using FluentValidation;
using static Houston.Core.Commands.PipelineTriggerCommands.CreatePipelineTriggerCommand;

namespace Houston.API.Validators.PipelineTriggerValidators {
	public class CreatePipelineTriggerEventFiltersValidator : AbstractValidator<CreatePipelineTriggerEventFilters> {
		public CreatePipelineTriggerEventFiltersValidator() {
			RuleForEach(x => x.FilterValues)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
