using Houston.Application.CommandHandlers.UserCommandHandlers.CreateSetup;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class CreateFirstSetupCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IOptions<AppConfiguration>> _mockOptions = new();
		private readonly IDistributedCache _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithSystemAlreadyConfigured_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new CreateFirstSetupCommandHandler(_mockUnitOfWork.Object, _cache, _mockOptions.Object);
			var command = _fixture.Create<CreateFirstSetupCommand>();
			var options = _fixture.Build<AppConfiguration>().With(x => x.ConfigurationKey, "configurations").Create();
			await _cache.SetStringAsync("configurations", JsonSerializer.Serialize(new SystemConfiguration()), default);
			_mockOptions.Setup(x => x.Value).Returns(options);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("The system has already been set up and configured.");
			errorResult?.ErrorCode.Should().Contain("alreadyConfigured");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithAnyUser_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new CreateFirstSetupCommandHandler(_mockUnitOfWork.Object, _cache, _mockOptions.Object);
			var command = _fixture.Create<CreateFirstSetupCommand>();
			var options = _fixture.Create<AppConfiguration>();
			_mockUnitOfWork.Setup(x => x.UserRepository.AnyUser()).ReturnsAsync(true);
			_mockOptions.Setup(x => x.Value).Returns(options);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("A user has already been registered in the system.");
			errorResult?.ErrorCode.Should().Contain("userAlreadyRegistered");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var handler = new CreateFirstSetupCommandHandler(_mockUnitOfWork.Object, _cache, _mockOptions.Object);
			var command = _fixture.Create<CreateFirstSetupCommand>();
			var options = _fixture.Create<AppConfiguration>();
			await _cache.RemoveAsync("configurations");
			_mockUnitOfWork.Setup(x => x.UserRepository.AnyUser()).ReturnsAsync(false);
			_mockOptions.Setup(x => x.Value).Returns(options);

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
