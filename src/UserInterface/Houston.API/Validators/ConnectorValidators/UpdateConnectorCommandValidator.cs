﻿using FluentValidation;
using Houston.Core.Commands.ConnectorCommands;

namespace Houston.API.Validators.ConnectorValidators {
	public class UpdateConnectorCommandValidator : AbstractValidator<UpdateConnectorCommand> {
		public UpdateConnectorCommandValidator() {
			RuleFor(x => x.ConnectorId)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);

			RuleFor(x => x.Name)
				.NotNull().NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(50).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Description)
				.MaximumLength(5000).WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
