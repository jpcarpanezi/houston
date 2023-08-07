using Houston.Application.CommandHandlers.AuthCommandHandlers.SignIn;

namespace Houston.API.UnitTests.HandlerTests.AuthCommandHandlers {
	[TestFixture]
	public class SignInCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();
		private IDistributedCache _cache;
		private SigningConfigurations _signingConfigurations;
		private TokenConfigurations _tokenConfigurations;

		[SetUp]
		public void SetUp() {
			_signingConfigurations = new SigningConfigurations(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PrivateKey.pem")));
			_tokenConfigurations = new TokenConfigurations("houston", "houston", 1200, 3600);
			_cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
		}

		[Test]
		public async Task Handle_WithNotFoundUser_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new SignInCommandHandler(_mockUnitOfWork.Object, _signingConfigurations, _tokenConfigurations, _cache);
			var command = _fixture.Create<SignInCommand>();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync((User?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid username or password.");
			errorResult?.ErrorCode.Should().Be("invalidCredentials");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithWrongPassword_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new SignInCommandHandler(_mockUnitOfWork.Object, _signingConfigurations, _tokenConfigurations, _cache);
			var command = _fixture.Create<SignInCommand>();
			var user = _fixture.Build<User>()
					  .OmitAutoProperties()
					  .With(x => x.Password, PasswordService.HashPassword("Abcd1234"))
					  .Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid username or password.");
			errorResult?.ErrorCode.Should().Be("invalidCredentials");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithInactiveUser_ShouldReturnUnauthorizedObject() {
			// Arrange
			var handler = new SignInCommandHandler(_mockUnitOfWork.Object, _signingConfigurations, _tokenConfigurations, _cache);
			var command = _fixture.Build<SignInCommand>().With(x => x.Password, "Abcd1234").Create();
			var user = _fixture.Build<User>()
					  .OmitAutoProperties()
					  .With(x => x.Password, PasswordService.HashPassword("Abcd1234"))
					  .With(x => x.Active, false)
					  .Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
			errorResult?.ErrorMessage.Should().Be("The account has been deactivated.");
			errorResult?.ErrorCode.Should().Be("userInactive");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithFirstAccess_ShouldReturnTemporaryRedirectObject() {
			// Arrange
			var handler = new SignInCommandHandler(_mockUnitOfWork.Object, _signingConfigurations, _tokenConfigurations, _cache);
			var command = _fixture.Build<SignInCommand>().With(x => x.Password, "Abcd1234").Create();
			var user = _fixture.Build<User>()
					  .OmitAutoProperties()
					  .With(x => x.Password, PasswordService.HashPassword("Abcd1234"))
					  .With(x => x.Active, true)
					  .With(x => x.FirstAccess, true)
					  .Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.TemporaryRedirect);
			errorResult?.CustomBody.Should().BeOfType<FirstAccessViewModel>();
			errorResult?.ErrorMessage.Should().BeEmpty();
			errorResult?.ErrorCode.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new SignInCommandHandler(_mockUnitOfWork.Object, _signingConfigurations, _tokenConfigurations, _cache);
			var command = _fixture.Build<SignInCommand>().With(x => x.Password, "Abcd1234").Create();
			var user = _fixture.Build<User>()
					  .OmitAutoProperties()
					  .With(x => x.Password, PasswordService.HashPassword("Abcd1234"))
					  .With(x => x.Active, true)
					  .With(x => x.FirstAccess, false)
					  .With(x => x.Name, "John Doe")
					  .With(x => x.Email, "john.doe@example.com")
					  .Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<BearerTokenViewModel, BearerTokenViewModel>>();

			var successResult = result as SuccessResultCommand<BearerTokenViewModel, BearerTokenViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}
