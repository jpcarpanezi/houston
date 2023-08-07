using Houston.Application.CommandHandlers.UserCommandHandlers.Create;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class CreateUserCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new Fixture();

		[Test]
		public async Task Handle_WithExistingUser_ShouldReturnConflictObject() {
			// Arrange
			var handler = new CreateUserCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<CreateUserCommand>();
			var user = _fixture.Build<User>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Conflict);
			errorResult?.ErrorMessage.Should().Be("A user with this email address already exists in the system.");
			errorResult?.ErrorCode.Should().Be("userAlreadyExists");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var handler = new CreateUserCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<CreateUserCommand>();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync((User?)null);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.UserRepository.Add(It.IsAny<User>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<User, UserViewModel>>();

			var successResult = result as SuccessResultCommand<User, UserViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Created);
			successResult?.Response.Should().BeOfType<User>();
		}
	}
}
