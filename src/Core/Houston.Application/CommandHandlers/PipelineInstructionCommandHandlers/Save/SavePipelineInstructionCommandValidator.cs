namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.Save {
	public class SavePipelineInstructionCommandValidator : AbstractValidator<SavePipelineInstructionCommand> {
		public SavePipelineInstructionCommandValidator() {
			RuleFor(x => x.PipelineInstructions)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(x => x.OrderBy(y => y.ConnectedToArrayIndex).First().ConnectedToArrayIndex == null).WithMessage("The first instruction in the pipeline must not have any connections.")
				.Must(x => x.Count == 1 || x.OrderBy(y => y.ConnectedToArrayIndex).Skip(1).Select((x, i) => x.ConnectedToArrayIndex == i).Contains(true)).WithMessage("The pipeline instructions are not connected in the correct order.");

			RuleForEach(x => x.PipelineInstructions).SetValidator(new SavePipelineInstructionValidator());
		}
	}
}
