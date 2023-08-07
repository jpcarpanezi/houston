using Houston.Application.CommandHandlers.UserCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class GetUserCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithUserNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new GetUserCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetUserCommand>();
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
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new GetUserCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetUserCommand>();
			var user = _fixture.Build<User>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<User, UserViewModel>>();

			var successResult = result as SuccessResultCommand<User, UserViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(user);
		}
	}
}
