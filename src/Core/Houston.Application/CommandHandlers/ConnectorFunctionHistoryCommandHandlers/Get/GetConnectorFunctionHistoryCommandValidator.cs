namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Get {
	public class GetConnectorFunctionHistoryCommandValidator : AbstractValidator<GetConnectorFunctionHistoryCommand> {
		public GetConnectorFunctionHistoryCommandValidator() {
			RuleFor(x => x.Id)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
