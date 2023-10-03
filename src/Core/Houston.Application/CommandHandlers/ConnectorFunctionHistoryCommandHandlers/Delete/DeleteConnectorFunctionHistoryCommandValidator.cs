namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Delete {
	public class DeleteConnectorFunctionHistoryCommandValidator : AbstractValidator<DeleteConnectorFunctionHistoryCommand> {
		public DeleteConnectorFunctionHistoryCommandValidator() {
			RuleFor(x => x.Id)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
