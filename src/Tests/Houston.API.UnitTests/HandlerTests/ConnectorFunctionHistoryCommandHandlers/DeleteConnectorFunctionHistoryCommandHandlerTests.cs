using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Delete;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionHistoryCommandHandlers {
	[TestFixture]
	public class DeleteConnectorFunctionHistoryCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims= new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithInactiveConnectorFunctionHistory_ReturnsNotFoundObject() {
			// Arrange
			var handler = new DeleteConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<DeleteConnectorFunctionHistoryCommand>();
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
		public async Task Handle_WithValidRequest_ShouldReturnNoContent() {
			// Arrange
			var handler = new DeleteConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<DeleteConnectorFunctionHistoryCommand>();
			var connectorFunctionHistory = _fixture.Build<ConnectorFunctionHistory>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connectorFunctionHistory);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionHistoryRepository.Update(It.IsAny<ConnectorFunctionHistory>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
