using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.MongoDB;
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
			var userId = ObjectId.GenerateNewId();
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
			var userId = ObjectId.GenerateNewId();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(userId)).ReturnsAsync((User)null!);

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
			var userId = ObjectId.GenerateNewId();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			var user = new User(userId, "John Doe", "john.doe@example.com", PasswordService.HashPassword("oldPassword"), false, Core.Enums.UserRoleEnum.User, false, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(userId)).ReturnsAsync(user);

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
			var userId = ObjectId.GenerateNewId();
			var command = new UpdatePasswordCommand(null, "wrongOldPassword", "StrongPassword1234");
			var user = new User(userId, "John Doe", "john.doe@example.com", PasswordService.HashPassword("oldPassword"), false, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.User });
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync(user);

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
			var userId = ObjectId.GenerateNewId();
			var command = new UpdatePasswordCommand(userId, null, "weakpwd");
			var user = new User(userId, "John Doe", "john.doe@example.com", PasswordService.HashPassword("oldPassword"), false, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(userId)).ReturnsAsync(user);

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
			var userId = ObjectId.GenerateNewId();
			var command = new UpdatePasswordCommand(userId, null, "StrongPassword1234");
			var user = new User(userId, "John Doe", "john.doe@example.com", PasswordService.HashPassword("oldPassword"), false, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUserClaimsService.Setup(x => x.Roles).Returns(new List<Core.Enums.UserRoleEnum> { Core.Enums.UserRoleEnum.Admin });
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(userId)).ReturnsAsync(user);

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
