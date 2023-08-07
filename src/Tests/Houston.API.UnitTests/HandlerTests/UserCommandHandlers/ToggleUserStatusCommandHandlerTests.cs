using Houston.Application.CommandHandlers.UserCommandHandlers.ToggleStatus;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class ToggleUserStatusCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithSelfUpdate_ShouldReturnForbiddenObject() {
			// Arrange
			Guid userId = Guid.NewGuid();
			var handler = new ToggleUserStatusCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<ToggleUserStatusCommand>().With(x => x.UserId, userId).Create();
			_mockClaims.Setup(x => x.Id).Returns(command.UserId);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Self-updating is not allowed for this resource.");
			errorResult?.ErrorCode.Should().Be("selfUpdatingNotAllowed");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithUserNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			Guid userId = Guid.NewGuid();
			var handler = new ToggleUserStatusCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<ToggleUserStatusCommand>().With(x => x.UserId, userId).Create();
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested user could not be found.");
			errorResult?.ErrorCode.Should().Be("userNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContentObject() {
			// Arrange
			Guid userId = Guid.NewGuid();
			var handler = new ToggleUserStatusCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Build<ToggleUserStatusCommand>().With(x => x.UserId, userId).Create();
			var user = _fixture.Build<User>().OmitAutoProperties().Create();
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.UserRepository.Update(It.IsAny<User>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
