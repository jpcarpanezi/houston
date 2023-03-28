using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Services;
using System.Net;

namespace Houston.API.UnitTests.UserCommandHandlers {
	[TestFixture]
	public class UpdateFirstAccessPasswordCommandHandlerTests {
		private UpdateFirstAccessPasswordCommandHandler _handler;
		private IDistributedCache _cache;
		private Mock<IUnitOfWork> _mockUnitOfWork;

		[SetUp] 
		public void SetUp() {
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
			_handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
		}

		[Test]
		public async Task Handle_WithUserEmailNotFound_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync((User)null!);

			// Act
			var result = await _handler.Handle(command, default);
			
			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithNoFirstAccessUserOrDisabled_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			var user = new User(userId, "John Doe", "john.doe@test.com", "StrongPassword1234", false, Core.Enums.UserRoleEnum.User, false, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithTokenNotFound_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			var user = new User(userId, "John Doe", "john.doe@test.com", "StrongPassword1234", true, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithInvalidToken_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			var user = new User(userId, "John Doe", "john.doe@test.com", "StrongPassword1234", true, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_cache.SetString(userId.ToString(), Guid.NewGuid().ToString("N"), new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithWeakPassword_ReturnsBadRequestAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var token = Guid.NewGuid().ToString("N");
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", token, "weakpwd");
			var user = new User(userId, "John Doe", "john.doe@test.com", PasswordService.HashPassword("tempStrongPwd1234"), true, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_cache.SetString(userId.ToString(), token, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

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
		public async Task Handle_WithInvalidTempPassword_ReturnsBadRequestAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var token = Guid.NewGuid().ToString("N");
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", token, "StrongPassword1324");
			var user = new User(userId, "John Doe", "john.doe@test.com", PasswordService.HashPassword("StrongPassword1324"), true, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_cache.SetString(userId.ToString(), token, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
				Assert.That(result.ErrorMessage, Is.EqualTo("passwordEqualToTemp"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsNoContent() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var token = Guid.NewGuid().ToString("N");
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", token, "StrongPassword1234");
			var user = new User(userId, "John Doe", "john.doe@test.com", PasswordService.HashPassword("StrongPassword1324"), true, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_cache.SetString(userId.ToString(), token, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.UserRepository.ReplaceOneAsync(user));
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Null);
			});
		}
	}
}
