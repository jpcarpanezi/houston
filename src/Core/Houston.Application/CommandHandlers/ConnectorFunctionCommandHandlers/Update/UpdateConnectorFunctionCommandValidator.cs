namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public class UpdateConnectorFunctionCommandValidator : AbstractValidator<UpdateConnectorFunctionCommand> {
		public UpdateConnectorFunctionCommandValidator() {
			RuleFor(x => x.Id)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
