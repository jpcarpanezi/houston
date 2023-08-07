using Houston.Application.CommandHandlers.UserCommandHandlers.UpdateFirstAccess;
using Houston.Core.Services;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class UpdateFirstAccessPasswordCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly IDistributedCache _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithUserNotFound_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Create<UpdateFirstAccessPasswordCommand>();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync((User?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid token.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithNotFirstAccess_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Create<UpdateFirstAccessPasswordCommand>();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.FirstAccess, false).With(x => x.Active, true).Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid token.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithInactiveUser_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Create<UpdateFirstAccessPasswordCommand>();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.FirstAccess, true).With(x => x.Active, false).Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid token.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithTokenNotFound_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Create<UpdateFirstAccessPasswordCommand>();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.FirstAccess, true).With(x => x.Active, true).Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid token.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithInvalidToken_ShouldReturnForbiddenObject() {
			// Arrange
			Guid userId = Guid.NewGuid();
			Guid token = Guid.NewGuid();
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Build<UpdateFirstAccessPasswordCommand>().With(x => x.Token, Guid.NewGuid().ToString()).Create();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.FirstAccess, true).With(x => x.Active, true).With(x => x.Id, userId).Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);
			_cache.SetString(userId.ToString(), token.ToString());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("Invalid token.");
			errorResult?.ErrorCode.Should().Be("invalidToken");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithEqualsPassword_ShouldReturnBadRequestObject() {
			// Arrange
			Guid userId = Guid.NewGuid();
			Guid token = Guid.NewGuid();
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Build<UpdateFirstAccessPasswordCommand>().With(x => x.Token, token.ToString()).With(x => x.Password, "TestPassword").Create();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.FirstAccess, true).With(x => x.Active, true).With(x => x.Id, userId).With(x => x.Password, PasswordService.HashPassword("TestPassword")).Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);
			_cache.SetString(userId.ToString(), token.ToString());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			errorResult?.ErrorMessage.Should().Be("The new password cannot be the same as previous one.");
			errorResult?.ErrorCode.Should().Be("passwordEqualToTemp");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContent() {
			// Arrange
			Guid userId = Guid.NewGuid();
			Guid token = Guid.NewGuid();
			var handler = new UpdateFirstAccessPasswordCommandHandler(_mockUnitOfWork.Object, _cache);
			var command = _fixture.Build<UpdateFirstAccessPasswordCommand>().With(x => x.Token, token.ToString()).With(x => x.Password, "NewPassword").Create();
			var user = _fixture.Build<User>().OmitAutoProperties().With(x => x.FirstAccess, true).With(x => x.Active, true).With(x => x.Id, userId).With(x => x.Password, PasswordService.HashPassword("TestPassword")).Create();
			_mockUnitOfWork.Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);
			_cache.SetString(userId.ToString(), token.ToString());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.UserRepository.Update(It.IsAny<User>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
