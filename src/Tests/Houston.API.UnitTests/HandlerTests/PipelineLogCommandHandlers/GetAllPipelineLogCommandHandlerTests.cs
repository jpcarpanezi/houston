using Houston.Application.CommandHandlers.PipelineLogCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.PipelineLogCommandHandlers {
	public class GetAllPipelineLogCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();
		private GetAllPipelineLogCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new GetAllPipelineLogCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var command = _fixture.Create<GetAllPipelineLogCommand>();
			var pipelineLogs = _fixture.Build<PipelineLog>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.PipelineLogsRepository.GetAllByPipelineId(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pipelineLogs);
			_mockUnitOfWork.Setup(x => x.PipelineLogsRepository.CountByPipelineId(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<long>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<PaginatedResultCommand<PipelineLog, PipelineLogViewModel>>();

			var paginatedResult = result as PaginatedResultCommand<PipelineLog, PipelineLogViewModel>;
			paginatedResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			paginatedResult?.Count.Should().Be(It.IsAny<long>());
			paginatedResult?.PageSize.Should().Be(command.PageSize);
			paginatedResult?.PageIndex.Should().Be(command.PageIndex);
			paginatedResult?.Response.Should().BeSameAs(pipelineLogs);
		}
	}
}
