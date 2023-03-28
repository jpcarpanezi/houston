using Houston.Application.CommandHandlers.UserCommandHandlers;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Interfaces.Services;
using System.Net;

namespace Houston.API.UnitTests.UserCommandHandlers {
	[TestFixture]
	public class CreateUserCommandHandlerTests {
		private CreateUserCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() {
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new CreateUserCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		public async Task Handle_WithExistingUser_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new CreateUserCommand("John Doe", "john.doe@example.com", "StrongPassword1234", Core.Enums.UserRoleEnum.User);
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(new User());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("userAlreadyExists"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithWeakPassword_ReturnsBadRequestAndErrorMessage() {
			// Arrange
			var command = new CreateUserCommand("John Doe", "john.doe@example.com", "weakpwd", Core.Enums.UserRoleEnum.User);
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync((User)null!);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
				Assert.That(result.ErrorMessage, Is.EqualTo("weakPassword"));
				Assert.That(result.Response, Is.EqualTo(null));
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsCreatedAndObject() {
			// Arrange
			var creatorId = ObjectId.GenerateNewId();
			var userId = ObjectId.GenerateNewId();
			var user = new User(userId, "John Doe", "john.doe@example.com", "StrongPassword1234", true, Core.Enums.UserRoleEnum.User, true, creatorId, It.IsAny<DateTime>(), creatorId, It.IsAny<DateTime>()) ;
			var command = new CreateUserCommand("John Doe", "john.doe@example.com", "StrongPassword1234", Core.Enums.UserRoleEnum.User);
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync((User)null!);
			_mockUserClaimsService.Setup(x => x.Id).Returns(creatorId);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.UserRepository.InsertOneAsync(It.IsAny<User>()), Times.Once);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Not.Null);
				Assert.That(result.Response?.Name, Is.EqualTo(user.Name));
				Assert.That(result.Response?.Email, Is.EqualTo(user.Email));
				Assert.That(result.Response?.Password, Is.Not.Null);
				Assert.That(result.Response?.UserRole, Is.EqualTo(user.UserRole));
				Assert.That(result.Response?.IsFirstAccess, Is.EqualTo(user.IsFirstAccess));
				Assert.That(result.Response?.IsActive, Is.EqualTo(user.IsActive));
				Assert.That(result.Response?.CreatedBy, Is.EqualTo(user.CreatedBy));
				Assert.That(result.Response?.CreationDate, Is.Not.Null);
				Assert.That(result.Response?.UpdatedBy, Is.EqualTo(user.UpdatedBy));
				Assert.That(result.Response?.LastUpdate, Is.Not.Null);
			});
		}
	}
}
