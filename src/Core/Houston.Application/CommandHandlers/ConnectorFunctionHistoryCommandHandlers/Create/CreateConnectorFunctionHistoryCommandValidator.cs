namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Create {
	public class CreateConnectorFunctionHistoryCommandValidator : AbstractValidator<CreateConnectorFunctionHistoryCommand> {
		public CreateConnectorFunctionHistoryCommandValidator() {
			RuleFor(x => x.ConnectorFunctionId)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);

			RuleFor(x => x.Version)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(IsValidVersionFormat).WithMessage("The version must be in the format of 'x.x.x'");

			RuleForEach(x => x.Script)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);

			RuleForEach(x => x.Package)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);

			RuleForEach(x => x.Inputs).SetValidator(new CreateConnectorFunctionInputCommandValidator());
		}

		private bool IsValidVersionFormat(string version) {
			if (string.IsNullOrEmpty(version)) return false;

			return Regex.IsMatch(version, @"^\d+\.\d+\.\d+$");
		}
	}

	public class CreateConnectorFunctionInputCommandValidator : AbstractValidator<CreateConnectorFunctionInputCommand> {
		public CreateConnectorFunctionInputCommandValidator() {
			RuleFor(x => x.Name)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Placeholder)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Replace)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Matches("^[a-zA-Z-]*$").WithMessage("The input does not match the required pattern. Only alphabetical characters and hyphens are allowed.")
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.DefaultValue)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Values)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.InputType)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);

			RuleFor(x => x.Required)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);

			RuleFor(x => x.AdvancedOption)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
