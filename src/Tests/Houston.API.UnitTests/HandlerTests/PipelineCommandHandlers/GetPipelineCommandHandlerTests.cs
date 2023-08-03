using Houston.Application.CommandHandlers.PipelineCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	[TestFixture]
	public class GetPipelineCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();
		private GetPipelineCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new GetPipelineCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithPipelineNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Create<GetPipelineCommand>();
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
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var command = _fixture.Create<GetPipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<Pipeline, PipelineViewModel>>();

			var successResult = result as SuccessResultCommand<Pipeline, PipelineViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipeline);
		}
	}
}
