namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class CreateConnectorFunctionCommandValidator : AbstractValidator<CreateConnectorFunctionCommand> {
		public CreateConnectorFunctionCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Inputs)
				.SetValidator(new CreateConnectorFunctionInputCommandValidator());

			RuleFor(x => x.Version)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(IsValidVersionFormat).WithMessage("The version must be in the format of 'x.x.x'");

			RuleForEach(x => x.Script)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);

			RuleForEach(x => x.Package)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}

		private bool IsValidVersionFormat(string version) {
			return Regex.IsMatch(version, @"^\d+\.\d+\.\d+$");
		}
	}
}
