using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Delete;

namespace Houston.API.UnitTests.HandlerTests.PipelineTriggerCommandHandlers {
	[TestFixture]
	public class DeletePipelineTriggerCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithPipelineTriggerNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new DeletePipelineTriggerCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<DeletePipelineTriggerCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PipelineTrigger?)null);

			// Act
			var result = await handler.Handle(command, default);

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
			var handler = new DeletePipelineTriggerCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<DeletePipelineTriggerCommand>();
			var pipelineTrigger = _fixture.Build<PipelineTrigger>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(pipelineTrigger);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineTriggerRepository.Remove(It.IsAny<PipelineTrigger>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
