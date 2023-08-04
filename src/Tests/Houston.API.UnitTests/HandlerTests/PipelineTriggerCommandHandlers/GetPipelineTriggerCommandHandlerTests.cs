using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.PipelineTriggerCommandHandlers {
	[TestFixture]
	public class GetPipelineTriggerCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithPipelineTriggerNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new GetPipelineTriggerCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetPipelineTriggerCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync((PipelineTrigger?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.ErrorMessage.Should().Be("The requested pipeline trigger could not be found.");
			errorResult?.ErrorCode.Should().Be("pipelineTriggerNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new GetPipelineTriggerCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetPipelineTriggerCommand>();
			var pipelineTrigger = _fixture.Build<PipelineTrigger>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(pipelineTrigger);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<PipelineTrigger, PipelineTriggerViewModel>>();

			var successResult = result as SuccessResultCommand<PipelineTrigger, PipelineTriggerViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipelineTrigger);
		}
	}
}
