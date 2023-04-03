using FluentValidation;
using Houston.Core.Commands.ConnectorFunctionCommands;

namespace Houston.API.Validators.ConnectorFunctionValidators {
	public class CreateConnectorFunctionCommandValidator : AbstractValidator<CreateConnectorFunctionCommand> {
		public CreateConnectorFunctionCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Inputs)
				.SetValidator(new GeneralConnectorFunctionInputCommandValidator());

			RuleForEach(x => x.Script)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
