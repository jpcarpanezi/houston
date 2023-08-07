using Houston.Application.CommandHandlers.PipelineLogCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.PipelineLogCommandHandlers {
	[TestFixture]
	public class GetPipelineLogCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithNotFoundPipelineLog_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new GetPipelineLogCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetPipelineLogCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineLogsRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync((PipelineLog?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested pipeline log could not be found.");
			errorResult?.ErrorCode.Should().Be("pipelineLogNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new GetPipelineLogCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetPipelineLogCommand>();
			var pipelineLog = _fixture.Build<PipelineLog>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineLogsRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(pipelineLog);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<PipelineLog, PipelineLogViewModel>>();

			var successResult = result as SuccessResultCommand<PipelineLog, PipelineLogViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipelineLog);
		}
	}
}
