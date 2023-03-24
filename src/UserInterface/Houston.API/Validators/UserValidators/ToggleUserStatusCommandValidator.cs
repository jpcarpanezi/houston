using FluentValidation;
using Houston.Core.Commands.UserCommands;
using MongoDB.Bson;

namespace Houston.API.Validators.UserValidators {
	public class ToggleUserStatusCommandValidator : AbstractValidator<ToggleUserStatusCommand> {
		public ToggleUserStatusCommandValidator() {
			RuleFor(x => x.UserId)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
