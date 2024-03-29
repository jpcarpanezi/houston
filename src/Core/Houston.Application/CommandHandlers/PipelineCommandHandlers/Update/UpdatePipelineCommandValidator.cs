﻿namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Update {
	public class UpdatePipelineCommandValidator : AbstractValidator<UpdatePipelineCommand> {
		public UpdatePipelineCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
