namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class CreateConnectorFunctionCommandValidator : AbstractValidator<CreateConnectorFunctionCommand> {
		public CreateConnectorFunctionCommandValidator() {
			RuleFor(x => x.SpecFile)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(x => x.Length <= 10 * 1024 * 1024).WithMessage("File size must be less than 10MB")
				.Must(x => x.Length > 0).WithMessage("File size must be greater than 0")
				.Must(x => x.ContentType == "application/x-yaml").WithMessage("File must be a YAML file");

			RuleFor(x => x.Script)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(x => x.Length <= 10 * 1024 * 1024).WithMessage("File size must be less than 10MB")
				.Must(x => x.Length > 0).WithMessage("File size must be greater than 0")
				.Must(x => x.ContentType == "application/x-yaml").WithMessage("File must be a YAML file");

			RuleFor(x => x.Package)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.Must(x => x.Length <= 10 * 1024 * 1024).WithMessage("File size must be less than 10MB")
				.Must(x => x.Length > 0).WithMessage("File size must be greater than 0")
				.Must(x => x.ContentType == "application/x-yaml").WithMessage("File must be a YAML file");
		}
	}
}
