using Houston.Application.CommandHandlers.UserCommandHandlers.UpdatePassword;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class UpdatePasswordCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithRoleNotAuthorized_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new UpdatePasswordCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<UpdatePasswordCommand>().With(x => x.UserId, Guid.NewGuid()).Create();
			_mockClaims.Setup(x => x.Roles).Returns(new List<UserRoleEnum> { UserRoleEnum.User });

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Only administators are allowed to change the password of other users.");
			errorResult?.ErrorCode.Should().Contain("unauthorizedPasswordChange");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithUserNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new UpdatePasswordCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<UpdatePasswordCommand>().With(x => x.UserId, (Guid?)null).Create();
			_mockClaims.Setup(x => x.Roles).Returns(new List<UserRoleEnum> { UserRoleEnum.User });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested user could not be found.");
			errorResult?.ErrorCode.Should().Contain("userNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithUserInactive_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new UpdatePasswordCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<UpdatePasswordCommand>().With(x => x.UserId, (Guid?)null).Create();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.Active, false).Create();
			_mockClaims.Setup(x => x.Roles).Returns(new List<UserRoleEnum> { UserRoleEnum.User });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("This user account is inactive.");
			errorResult?.ErrorCode.Should().Contain("inactiveUser");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithIncorrectOldPassword_ShouldReturnBadRequestObject() {
			// Arrange
			var handler = new UpdatePasswordCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<UpdatePasswordCommand>().With(x => x.UserId, (Guid?)null).Create();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.Active, true).With(x => x.Password, PasswordService.HashPassword("OldPassword")).Create();
			_mockClaims.Setup(x => x.Roles).Returns(new List<UserRoleEnum> { UserRoleEnum.User });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			errorResult?.ErrorMessage.Should().Be("The old password provided is incorrect.");
			errorResult?.ErrorCode.Should().Contain("incorrectOldPassword");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContentObject() {
			// Arrange
			var handler = new UpdatePasswordCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<UpdatePasswordCommand>().With(x => x.UserId, (Guid?)null).With(x => x.OldPassword, "OldPassword").Create();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.Active, true).With(x => x.Password, PasswordService.HashPassword("OldPassword")).Create();
			_mockClaims.Setup(x => x.Roles).Returns(new List<UserRoleEnum> { UserRoleEnum.User });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand>();
			
			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
