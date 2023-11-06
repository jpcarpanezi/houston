namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public class UpdateConnectorFunctionCommandValidator : AbstractValidator<UpdateConnectorFunctionCommand> {
		public UpdateConnectorFunctionCommandValidator() {
			RuleFor(x => x.Id)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);

			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
