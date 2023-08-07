using Houston.Application.CommandHandlers.PipelineCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	[TestFixture]
	public class GetAllPipelineCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkPaginatedObject() {
			// Arrange
			var handler = new GetAllPipelineCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetAllPipelineCommand>();
			var pipelines = _fixture.Build<Pipeline>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetAllActives(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pipelines);
			_mockUnitOfWork.Setup(x => x.PipelineRepository.CountActives()).ReturnsAsync(It.IsAny<long>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<PaginatedResultCommand<Pipeline, PipelineViewModel>>();

			var paginatedResult = result as PaginatedResultCommand<Pipeline, PipelineViewModel>;
			paginatedResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			paginatedResult?.Response.Should().BeSameAs(pipelines);
			paginatedResult?.PageSize.Should().Be(command.PageSize);
			paginatedResult?.PageIndex.Should().Be(command.PageIndex);
			paginatedResult?.Count.Should().Be(It.IsAny<long>());
		}
	}
}
