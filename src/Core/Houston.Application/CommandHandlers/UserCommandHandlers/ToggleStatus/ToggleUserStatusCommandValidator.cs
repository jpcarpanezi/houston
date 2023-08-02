namespace Houston.Application.CommandHandlers.UserCommandHandlers.ToggleStatus {
	public class ToggleUserStatusCommandValidator : AbstractValidator<ToggleUserStatusCommand> {
		public ToggleUserStatusCommandValidator() {
			RuleFor(x => x.UserId)
				.NotNull().WithMessage(ValidatorsModelErrorMessages.Null);
		}
	}
}
