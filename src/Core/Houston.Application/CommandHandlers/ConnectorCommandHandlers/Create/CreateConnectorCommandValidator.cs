namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Create {
	public class CreateConnectorCommandValidator : AbstractValidator<CreateConnectorCommand> {
		public CreateConnectorCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(IsKebabCase).WithMessage("Function name must be in kebab case")
				.MaximumLength(64).WithMessage(ValidatorsModelErrorMessages.MaxLength);
			
			RuleFor(x => x.FriendlyName)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
		
		private static bool IsKebabCase(string value) {
			return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-z][a-z0-9-]*$");
		}
	}
}
