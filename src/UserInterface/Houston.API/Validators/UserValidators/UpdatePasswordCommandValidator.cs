using FluentValidation;
using Houston.Core.Commands.UserCommands;

namespace Houston.API.Validators.UserValidators {
	public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand> {
		public UpdatePasswordCommandValidator() {
			RuleFor(x => x.NewPassword)
				.NotNull()
				.NotEmpty()
				.WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.NotEqual(x => x.OldPassword)
				.WithMessage("newPasswordEqualToOld")
				.MaximumLength(64)
				.WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.OldPassword)
				.NotNull()
				.NotEmpty()
				.When(x => x.UserId is null)
				.WithMessage("oldPasswordIsRequiredWhenUserIdNotProvided")
				.MaximumLength(64)
				.WithMessage(ValidatorsModelErrorMessages.MaxLength);
		}
	}
}
