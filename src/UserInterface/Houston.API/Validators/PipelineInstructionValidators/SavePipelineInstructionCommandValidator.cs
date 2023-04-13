using FluentValidation;
using Houston.Core.Commands.PipelineInstructionCommands;

namespace Houston.API.Validators.PipelineInstructionValidators {
	public class SavePipelineInstructionCommandValidator : AbstractValidator<SavePipelineInstructionCommand> {
		public SavePipelineInstructionCommandValidator() {
			RuleFor(x => x.PipelineInstructions)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(x => x.OrderBy(y => y.ConnectedToArrayIndex).First().ConnectedToArrayIndex == null).WithMessage("firstInstructionMustNotHaveConnections")
				.Must(x => x.OrderBy(y => y.ConnectedToArrayIndex).Skip(1).Select((x, i) => x.ConnectedToArrayIndex == i).Contains(true)).WithMessage("instructionsNotConnectedInOrder");

			RuleForEach(x => x.PipelineInstructions).SetValidator(new PipelineInstructionValidator());
		}
	}
}
