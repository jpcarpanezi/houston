using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using System.Net;

namespace Houston.API.UnitTests.UserCommandHandlers {
	[TestFixture]
	public class CreateFirstSetupCommandHandlerTests {
		private CreateFirstSetupCommandHandler _handler;
		private IDistributedCache _cache;
		private Mock<IUnitOfWork> _mockUnitOfWork;

		private const string ConfigurationKey = "configurations";
		private const string DefaultOs = "debian";
		private const string DefaultOsVersion = "11.6";

		[SetUp] 
		public void SetUp() {
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
			_handler = new CreateFirstSetupCommandHandler(_mockUnitOfWork.Object, _cache);
		}

		[Test]
		public async Task Handle_WithExistingConfiguration_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new CreateFirstSetupCommand();
			var systemConfiguration = new SystemConfiguration("hub.docker.com", "test@test.com", "test", "password", DefaultOs, DefaultOsVersion, true);
			await _cache.SetStringAsync(ConfigurationKey, JsonSerializer.Serialize(systemConfiguration));

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("alreadyConfigured"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithWeakPassword_ReturnsBadRequestAndErrorMessage() {
			// Arrange
			var command = new CreateFirstSetupCommand("hub.docker.com", "test@test.com", "test", "password", "user", "test@test.com", "weakpwd");

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
				Assert.That(result.ErrorMessage, Is.EqualTo("weakPassword"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithUserAlreadyRegistered_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new CreateFirstSetupCommand("hub.docker.com", "test@test.com", "test", "password", "user", "test@test.com", "StrongPassword1234");
			_mockUnitOfWork.Setup(x => x.UserRepository.AnyUser()).ReturnsAsync(true);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("userAlreadyRegistered"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsCreatedAndObject() {
			// Arrange
			var command = new CreateFirstSetupCommand("hub.docker.com", "test@test.com", "test", "password", "user", "test@test.com", "StrongPassword1234");
			_mockUnitOfWork.Setup(x => x.UserRepository.AnyUser()).ReturnsAsync(false);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(result.ErrorMessage, Is.Null);
			});

			Assert.That(result.Response, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.Response.Name, Is.EqualTo(command.UserName));
				Assert.That(result.Response.Email, Is.EqualTo(command.UserEmail));
				Assert.That(result.Response.FirstAccess, Is.False);
				Assert.That(result.Response.Active, Is.True);
			});

			_mockUnitOfWork.Verify(x => x.UserRepository.Add(It.IsAny<User>()), Times.Once);
		}
	}
}
