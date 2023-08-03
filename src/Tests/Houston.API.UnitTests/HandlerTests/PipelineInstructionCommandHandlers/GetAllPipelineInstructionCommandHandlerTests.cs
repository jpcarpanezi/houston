using Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.PipelineInstructionCommandHandlers {
	[TestFixture]
	public class GetAllPipelineInstructionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();
		private GetAllPipelineInstructionCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new GetAllPipelineInstructionCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var command = _fixture.Create<GetAllPipelineInstructionCommand>();
			var pipelineInstructions = _fixture.Build<PipelineInstruction>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.PipelineInstructionRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(pipelineInstructions);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<List<PipelineInstruction>, List<PipelineInstructionViewModel>>>();

			var successResult = result as SuccessResultCommand<List<PipelineInstruction>, List<PipelineInstructionViewModel>>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipelineInstructions);
		}
	}
}
