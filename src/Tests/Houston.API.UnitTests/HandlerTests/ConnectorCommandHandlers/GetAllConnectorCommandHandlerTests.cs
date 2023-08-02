using Houston.Application.CommandHandlers.ConnectorCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.ConnectorCommandHandlers {
	[TestFixture]
	public class GetAllConnectorCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();
		private GetAllConnectorCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new GetAllConnectorCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkPaginatedObject() {
			// Arrange
			var command = _fixture.Create<GetAllConnectorCommand>();
			var connectors = _fixture.Build<Connector>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetAllActives(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(connectors);
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.CountActives()).ReturnsAsync(It.IsAny<long>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<PaginatedResultCommand<Connector, ConnectorViewModel>>();

			var paginatedResult = result as PaginatedResultCommand<Connector, ConnectorViewModel>;
			paginatedResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			paginatedResult?.Count.Should().Be(It.IsAny<long>());
			paginatedResult?.PageSize.Should().Be(command.PageSize);
			paginatedResult?.PageIndex.Should().Be(command.PageIndex);
			paginatedResult?.Response.Should().BeSameAs(connectors);
		}
	}
}
