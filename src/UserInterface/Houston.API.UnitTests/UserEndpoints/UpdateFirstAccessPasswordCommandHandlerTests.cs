using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Services;
using System.Net;

namespace Houston.API.UnitTests.UserEndpoints {
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
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithNoFirstAccessUserOrDisabled_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = false,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithTokenNotFound_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithInvalidToken_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			Guid userId = Guid.NewGuid();
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", Guid.NewGuid().ToString("N"), "StrongPassword1324");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_cache.SetString(userId.ToString(), Guid.NewGuid().ToString("N"), new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("invalidToken"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithInvalidTempPassword_ReturnsBadRequestAndErrorMessage() {
			// Arrange
			var userId = Guid.NewGuid();
			var token = Guid.NewGuid().ToString("N");
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", token, "StrongPassword1234");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("StrongPassword1234"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_cache.SetString(userId.ToString(), token, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("passwordEqualToTemp"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsNoContent() {
			// Arrange
			var userId = Guid.NewGuid();
			var token = Guid.NewGuid().ToString("N");
			var command = new UpdateFirstAccessPasswordCommand("john.doe@test.com", token, "StrongPassword1234");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("StrongTempPassword1234"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_cache.SetString(userId.ToString(), token, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(command.Email)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Null);
			});
			_mockUnitOfWork.Verify(x => x.UserRepository.Update(user), Times.Once);
		}
	}
}
