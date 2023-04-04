using FluentValidation;
using Houston.Core.Commands.ConnectorFunctionCommands;

namespace Houston.API.Validators.ConnectorFunctionValidators {
	public class GetAllConnectorFunctionCommandValidator : AbstractValidator<GetAllConnectorFunctionCommand> {
		public GetAllConnectorFunctionCommandValidator() {
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
