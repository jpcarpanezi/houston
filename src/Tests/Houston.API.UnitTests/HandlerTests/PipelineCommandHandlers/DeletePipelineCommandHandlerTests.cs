using Houston.Application.CommandHandlers.PipelineCommandHandlers.Delete;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	public class DeletePipelineCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private DeletePipelineCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new DeletePipelineCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WithPipelineNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Create<DeletePipelineCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync((Pipeline?)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested pipeline could not be found.");
			errorResult?.ErrorCode.Should().Be("pipelineNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithPipelineRunning_ShouldReturnLockedObject() {
			// Arrange
			var command = _fixture.Create<DeletePipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().With(x => x.Status, Core.Enums.PipelineStatusEnum.Running).Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockUnitOfWork.Setup(x => x.PipelineLogsRepository.DurationAverage(It.IsAny<Guid>(), default)).ReturnsAsync(It.IsAny<double>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Locked);
			errorResult?.ErrorMessage.Should().BeEmpty();
			errorResult?.ErrorCode.Should().BeNull();
			errorResult?.CustomBody.Should().BeOfType<LockedMessageViewModel>();

			var customBody = errorResult?.CustomBody as LockedMessageViewModel;
			customBody?.Message.Should().Be("Server is processing a request from this pipeline. Please try again later.");
			customBody?.ErrorCode.Should().Be("pipelineRunning");
			customBody?.EstimatedCompletionTime.Should().BeCloseTo(DateTime.UtcNow.AddTicks((long)It.IsAny<double>()), TimeSpan.FromSeconds(1));
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContent() {
			// Arrange
			var command = _fixture.Create<DeletePipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().With(x => x.Status, Core.Enums.PipelineStatusEnum.Awaiting).Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineRepository.Update(It.IsAny<Pipeline>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
