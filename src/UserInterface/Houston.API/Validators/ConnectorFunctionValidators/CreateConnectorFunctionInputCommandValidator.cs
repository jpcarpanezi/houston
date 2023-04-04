using FluentValidation;
using Houston.Core.Commands.ConnectorFunctionCommands;

namespace Houston.API.Validators.ConnectorFunctionValidators {
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
				.Matches("^[a-zA-Z-]*$").WithMessage("onlyLettersNumbersAndHyfens")
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.DefaultValue)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Values)
				.MaximumLength(100).WithMessage(ValidatorsModelErrorMessages.MaxLength);

		}
	}
}
