namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class ConnectorFunctionSpecValidator : AbstractValidator<ConnectorFunctionSpec> {
		public ConnectorFunctionSpecValidator() {
			RuleFor(x => x.Connector)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength)
				.Must(IsKebabCase).WithMessage("Connector name must be in kebab case");
			
			RuleFor(x => x.Function)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(IsKebabCase).WithMessage("Function name must be in kebab case");

			RuleFor(x => x.Version)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(IsValidVersionFormat).WithMessage("Version must be in the format of x.x.x");

			RuleFor(x => x.FriendlyName)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Runs).SetValidator(new ConnectorFunctionRunsSpecValidator());
		}
		
		private static bool IsValidVersionFormat(string value) {
			return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^\d+\.\d+\.\d+$");
		}
		
		private static bool IsKebabCase(string value) {
			return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-z][a-z0-9-]*$");
		}
	}

	public class ConnectorFunctionInputsValuesSpecValidator : AbstractValidator<ConnectorFunctionInputsSpec> {
		public ConnectorFunctionInputsValuesSpecValidator() {
			RuleFor(x => x.Type)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
			
			RuleFor(x => x.Label)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Placeholder)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Values)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Default)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Required)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);

			RuleFor(x => x.Advanced)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
	
	public class ConnectorFunctionRunsSpecValidator : AbstractValidator<ConnectorFunctionRunsSpec> {
		public ConnectorFunctionRunsSpecValidator() {
			RuleFor(x => x.Using)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
