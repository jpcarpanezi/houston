using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Interfaces.Services;
using System.Net;

namespace Houston.API.UnitTests.UserCommandHandlers {
	[TestFixture]
	public class ToggleUserStatusCommandHandlerTests {
		private ToggleUserStatusCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new ToggleUserStatusCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		public async Task Handle_WithSelfUpdate_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var command = new ToggleUserStatusCommand(userId);
			_mockUserClaimsService.Setup(x => x.Id).Returns(userId);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("selfUpdateNotAllowed"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithNonExistingUser_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var command = new ToggleUserStatusCommand(userId);
			_mockUserClaimsService.Setup(x => x.Id).Returns(It.IsAny<ObjectId>());
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(userId)).ReturnsAsync((User)null!);

			// Act
			var result = await _handler.Handle(command, default);
			// Assert

			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
				Assert.That(result.ErrorMessage, Is.EqualTo("userNotFound"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsNoContent() {
			// Arrange
			var userId = ObjectId.GenerateNewId();
			var command = new ToggleUserStatusCommand(userId);
			var user = new User(userId, "John Doe", "john.doe@example.com", "StrongPassword1234", false, Core.Enums.UserRoleEnum.User, true, It.IsAny<ObjectId>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<DateTime>());
			_mockUserClaimsService.Setup(x => x.Id).Returns(It.IsAny<ObjectId>());
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByIdAsync(userId)).ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.UserRepository.ReplaceOneAsync(user), Times.Once);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Null);
			});
		}
	}
}
