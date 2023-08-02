namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.GetAll {
	public class GetAllConnectorCommandValidator : AbstractValidator<GetAllConnectorCommand> {
		public GetAllConnectorCommandValidator() {
			RuleFor(x => x.PageIndex)
				.GreaterThanOrEqualTo(0)
				.WithMessage(ValidatorsModelErrorMessages.MinValue);

			RuleFor(x => x.PageSize)
				.GreaterThanOrEqualTo(1)
				.WithMessage(ValidatorsModelErrorMessages.MinValue)
				.LessThanOrEqualTo(100)
				.WithMessage(ValidatorsModelErrorMessages.MaxValue);
		}
	}
}
