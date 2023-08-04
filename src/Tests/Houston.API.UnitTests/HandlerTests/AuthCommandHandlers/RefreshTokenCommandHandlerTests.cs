using Houston.Application.CommandHandlers.AuthCommandHandlers.RefreshToken;

namespace Houston.API.UnitTests.HandlerTests.AuthCommandHandlers {
	[TestFixture]
	public class RefreshTokenCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly IDistributedCache _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
		private readonly Fixture _fixture = new();
		private RefreshTokenCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			var signingConfigurations = new SigningConfigurations(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PrivateKey.pem")));
			var tokenConfigurations = new TokenConfigurations("houston", "houston", 1200, 3600);
			_handler = new RefreshTokenCommandHandler(_mockUnitOfWork.Object, signingConfigurations, tokenConfigurations, _cache);
		}

		[Test]
		public async Task Handle_WithWhiteSpacedToken_ReturnsForbiddenObject() {
			// Arrange
			var command = _fixture.Build<RefreshTokenCommand>().With(x => x.Token, string.Empty).Create();

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid token.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithExpiredToken_ReturnsForbiddenObject() {
			// Arrange
			var command = _fixture.Create<RefreshTokenCommand>();

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Token expired.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithUserNotFound_ReturnsForbiddenObject() {
			// Arrange
			var command = _fixture.Create<RefreshTokenCommand>();
			var refreshTokenData = _fixture.Build<RefreshTokenData>().With(x => x.UserId, Guid.NewGuid().ToString()).Create();
			await _cache.SetStringAsync(command.Token, JsonSerializer.Serialize(refreshTokenData));
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("User not found.");
			errorResult?.ErrorCode.Should().Be("userNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithInactiveUser_ReturnsForbiddenObject() {
			// Arrange
			var command = _fixture.Create<RefreshTokenCommand>();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.Active, false).Create();
			var refreshTokenData = _fixture.Build<RefreshTokenData>().With(x => x.UserId, Guid.NewGuid().ToString()).Create();
			await _cache.SetStringAsync(command.Token, JsonSerializer.Serialize(refreshTokenData));
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("User inactive.");
			errorResult?.ErrorCode.Should().Be("userNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ReturnsOkObject() {
			// Arrange
			var command = _fixture.Create<RefreshTokenCommand>();
			var user = _fixture.Build<User>()
					  .OmitAutoProperties()
					  .With(x => x.Active, true)
					  .With(x => x.Name, "John Doe")
					  .With(x => x.Email, "john.doe@example.com")
					  .Create();
			var refreshTokenData = _fixture.Build<RefreshTokenData>().With(x => x.UserId, Guid.NewGuid().ToString()).Create();
			await _cache.SetStringAsync(command.Token, JsonSerializer.Serialize(refreshTokenData));
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<BearerTokenViewModel, BearerTokenViewModel>>();

			var successResult = result as SuccessResultCommand<BearerTokenViewModel, BearerTokenViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}
