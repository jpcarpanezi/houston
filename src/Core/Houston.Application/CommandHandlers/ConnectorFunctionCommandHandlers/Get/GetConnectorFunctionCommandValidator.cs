namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Get {
	public class GetConnectorFunctionCommandValidator : AbstractValidator<GetConnectorFunctionCommand> {
		public GetConnectorFunctionCommandValidator() {
			RuleFor(x => x.Id)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
