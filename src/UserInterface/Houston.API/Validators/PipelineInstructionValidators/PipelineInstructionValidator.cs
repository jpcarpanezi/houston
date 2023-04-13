using FluentValidation;
using static Houston.Core.Commands.PipelineInstructionCommands.SavePipelineInstructionCommand;

namespace Houston.API.Validators.PipelineInstructionValidators {
	public class PipelineInstructionValidator : AbstractValidator<PipelineInstruction> {
		public PipelineInstructionValidator() {
			RuleForEach(x => x.Script)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
