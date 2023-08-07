using Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.PipelineInstructionCommandHandlers {
	[TestFixture]
	public class GetAllPipelineInstructionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new GetAllPipelineInstructionCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetAllPipelineInstructionCommand>();
			var pipelineInstructions = _fixture.Build<PipelineInstruction>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.PipelineInstructionRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(pipelineInstructions);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<List<PipelineInstruction>, List<PipelineInstructionViewModel>>>();

			var successResult = result as SuccessResultCommand<List<PipelineInstruction>, List<PipelineInstructionViewModel>>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipelineInstructions);
		}
	}
}
