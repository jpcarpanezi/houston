using Houston.Application.CommandHandlers.PipelineCommandHandlers.Webhook;
using Houston.Application.Webhooks;
using Houston.Core.Interfaces.Services;
using Moq;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	public class WebhookCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IEventBus> _mockEventBus = new();
		private readonly Mock<IHttpContextAccessor> _mockContext = new();
		private readonly Mock<IWebhookService> _mockWebhookService = new();
		private readonly Fixture _fixture = new();
		private WebhookCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new WebhookCommandHandler(_mockUnitOfWork.Object, _mockEventBus.Object, _mockContext.Object);
		}

		[Test]
		public async Task Handle_WithWebhookOriginNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Build<WebhookCommand>().With(x => x.Origin, "GutHibs").Create();

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested webhook origin does not exists.");
			errorResult?.ErrorCode.Should().Be("invalidWebhookOrigin");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithInvalidPayload_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Build<WebhookCommand>().With(x => x.Origin, "GitHub").Create();
			_mockWebhookService.Setup(x => x.DeserializeOrigin(It.IsAny<string>())).Returns((string?)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested webhook origin does not exists.");
			errorResult?.ErrorCode.Should().Be("invalidWebhookOrigin");
			errorResult?.CustomBody.Should().BeNull();
		}
	}
}
