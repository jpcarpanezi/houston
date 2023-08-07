using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionCommandHandlers {
	[TestFixture]
	public class GetAllConnectorFunctionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkPaginatedResult() {
			// Arrange
			var handler = new GetAllConnectorFunctionCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetAllConnectorFunctionCommand>();
			var connectorFunctions = _fixture.Build<ConnectorFunction>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetAllActivesByConnectorId(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(connectorFunctions);
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.CountActivesByConnectorId(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<long>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<PaginatedResultCommand<ConnectorFunction, ConnectorFunctionViewModel>>();

			var paginatedResult = result as PaginatedResultCommand<ConnectorFunction, ConnectorFunctionViewModel>;
			paginatedResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			paginatedResult?.Count.Should().Be(It.IsAny<long>());
			paginatedResult?.PageSize.Should().Be(command.PageSize);
			paginatedResult?.PageIndex.Should().Be(command.PageIndex);
			paginatedResult?.Response.Should().BeSameAs(connectorFunctions);
		}
	}
}
