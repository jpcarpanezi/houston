using FluentValidation;
using Houston.Core.Commands.ConnectorFunctionCommands;

namespace Houston.API.Validators.ConnectorFunctionValidators {
	public class UpdateConnectorFunctionCommandValidator : AbstractValidator<UpdateConnectorFunctionCommand> {
		public UpdateConnectorFunctionCommandValidator() {
			RuleFor(x => x.Name)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(25).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleForEach(x => x.Inputs)
				.SetValidator(new UpdateConnectorFunctionInputCommandValidator());

			RuleForEach(x => x.Script)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
