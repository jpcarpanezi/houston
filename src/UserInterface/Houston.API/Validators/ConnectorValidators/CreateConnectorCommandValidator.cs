using FluentValidation;
using Houston.Core.Commands.ConnectorCommands;

namespace Houston.API.Validators.ConnectorValidators {
	public class CreateConnectorCommandValidator : AbstractValidator<CreateConnectorCommand> {
		public CreateConnectorCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().NotNull().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
