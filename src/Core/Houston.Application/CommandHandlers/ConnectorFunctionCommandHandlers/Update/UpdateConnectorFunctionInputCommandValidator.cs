namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public class UpdateConnectorFunctionInputCommandValidator : AbstractValidator<UpdateConnectorFunctionInputCommand> {
		public UpdateConnectorFunctionInputCommandValidator() {
			RuleFor(x => x.Name)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Placeholder)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Replace)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Matches("^[a-zA-Z-]*$").WithMessage("The input does not match the required pattern. Only alphabetical characters and hyphens are allowed.")
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.DefaultValue)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Values)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
