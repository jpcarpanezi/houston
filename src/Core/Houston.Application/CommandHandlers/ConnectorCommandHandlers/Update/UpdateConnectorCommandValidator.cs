namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Update {
	public class UpdateConnectorCommandValidator : AbstractValidator<UpdateConnectorCommand> {
		public UpdateConnectorCommandValidator() {
			RuleFor(x => x.ConnectorId)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
			
			RuleFor(x => x.FriendlyName)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
