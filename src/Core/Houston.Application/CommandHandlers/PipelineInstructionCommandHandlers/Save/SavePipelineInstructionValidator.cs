namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.Save {
	public class SavePipelineInstructionValidator : AbstractValidator<SavePipelineInstruction> {
		public SavePipelineInstructionValidator() {
			RuleForEach(x => x.Script)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
