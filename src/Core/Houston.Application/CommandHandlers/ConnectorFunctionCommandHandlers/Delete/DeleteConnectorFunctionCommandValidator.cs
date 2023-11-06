namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Delete {
	public class DeleteConnectorFunctionCommandValidator : AbstractValidator<DeleteConnectorFunctionCommand> {
		public DeleteConnectorFunctionCommandValidator() {
			RuleFor(x => x.Id)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
