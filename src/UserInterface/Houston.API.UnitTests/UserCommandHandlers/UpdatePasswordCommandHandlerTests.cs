using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using Houston.Core.Services;
using System.Net;

namespace Houston.API.UnitTests.UserCommandHandlers {
	[TestFixture]
	public class UpdatePasswordCommandHandlerTests {
		private UpdatePasswordCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new UpdatePasswordCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		public async Task Handle_WithUnauthorizedPasswordChange_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = Guid.NewGuid();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.User });

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("unauthorizedPasswordChange"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithNotFoundUser_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = Guid.NewGuid();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(default(User));

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidUser"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithInactiveUser_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = Guid.NewGuid();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("StrongPassword1234"),
				Active = false,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("inactiveUser"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithIncorrectOldPassword_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = Guid.NewGuid();
			var command = new UpdatePasswordCommand(null, "wrongOldPassword", "StrongPassword1234");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("StrongPassword1234"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.User });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("incorrectOldPassword"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithWeakPassword_ReturnsBadRequestAndErrorMessage() {
			// Arrange
			var userId = Guid.NewGuid();
			var command = new UpdatePasswordCommand(userId, null, "weakpwd");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("StrongPassword1234"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user);

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
		public async Task Handle_WithValidParameters_ReturnsNoContent() {
			// Arrange
			var userId = Guid.NewGuid();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			var user = new User {
				Id = userId,
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("StrongPassword1234"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Null);
			});
		}
	}
}
