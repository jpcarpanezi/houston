using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.ChangeSecret;

namespace Houston.API.UnitTests.HandlerTests.PipelineTriggerCommandHandlers {
	[TestFixture]
	public class ChangeSecretPipelineTriggerCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private ChangeSecretPipelineTriggerCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new ChangeSecretPipelineTriggerCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WithPipelineTriggerNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Create<ChangeSecretPipelineTriggerCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PipelineTrigger?)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested pipeline trigger could not be found.");
			errorResult?.ErrorCode.Should().Be("pipelineTriggerNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContentObject() {
			// Arrange
			var command = _fixture.Create<ChangeSecretPipelineTriggerCommand>();
			var pipelineTrigger = _fixture.Build<PipelineTrigger>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(pipelineTrigger);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineTriggerRepository.Update(It.IsAny<PipelineTrigger>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
