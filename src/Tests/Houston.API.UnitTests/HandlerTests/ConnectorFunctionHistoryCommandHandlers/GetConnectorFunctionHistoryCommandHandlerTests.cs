using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionHistoryCommandHandlers {
	[TestFixture]
	public class GetConnectorFunctionHistoryCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithInactiveConnectorFunctionHistory_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new GetConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<GetConnectorFunctionHistoryCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync((ConnectorFunctionHistory?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector function history could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorFunctionHistoryNotFound");
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new GetConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<GetConnectorFunctionHistoryCommand>();
			var connectorFunctionHistory = _fixture.Build<ConnectorFunctionHistory>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connectorFunctionHistory);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().Be(connectorFunctionHistory);
		}
	}
}
