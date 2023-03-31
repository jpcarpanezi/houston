using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using Houston.Core.Services;
using System.Net;

namespace Houston.API.UnitTests.UserEndpoints {
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
			Guid userId = Guid.NewGuid();
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
			Guid userId = Guid.NewGuid();
			var command = new ToggleUserStatusCommand(userId);
			_mockUserClaimsService.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync((User)null!);

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
			Guid userId = Guid.NewGuid();
			var command = new ToggleUserStatusCommand(userId);
			var user = new User {
				Id = Guid.NewGuid(),
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
			_mockUserClaimsService.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user);

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
