namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Create {
	public class CreateConnectorCommandValidator : AbstractValidator<CreateConnectorCommand> {
		public CreateConnectorCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
