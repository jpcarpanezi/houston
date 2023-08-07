using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.ConnectorCommandHandlers {
	[TestFixture]
	public class GetConnectorCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithNotFoundConnector_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new GetConnectorCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetConnectorCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync((Connector?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorNotFound");
			errorResult?.CustomBody.Should().BeNull();

		}

		[Test]
		public async Task Handle_WithValidConnector_ShouldReturnOkObject() {
			// Arrange
			var handler = new GetConnectorCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetConnectorCommand>();
			var connector = _fixture.Build<Connector>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<Connector, ConnectorViewModel>>();

			var successResult = result as SuccessResultCommand<Connector, ConnectorViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connector);
		}
	}
}
