using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Update;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionHistoryCommandHandlers {
	[TestFixture]
	public class UpdateConnectorFunctionHistoryCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Mock<IPublishEndpoint> _mockPublishEndpoint = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithConnectorFunctionNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new UpdateConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var command = _fixture.Create<UpdateConnectorFunctionHistoryCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.GetByIdWithInputs(It.IsAny<Guid>())).ReturnsAsync((ConnectorFunctionHistory?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector function could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorFunctionNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithBuildScript_ShouldReturnOkObject() {
			// Arrange
			var handler = new UpdateConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var commandScript = _fixture.CreateMany<byte>().ToArray();
			var commandPackage = _fixture.CreateMany<byte>().ToArray();
			var connectorFunctionHistoryScript = _fixture.CreateMany<byte>().ToArray();
			var connectorFunctionHistoryPackage = _fixture.CreateMany<byte>().ToArray();
			var command = _fixture.Build<UpdateConnectorFunctionHistoryCommand>().With(x => x.Script, commandScript).With(x => x.Package, commandPackage).Create();
			var connectorFunctionHistory = _fixture.Build<ConnectorFunctionHistory>().With(x => x.Script, connectorFunctionHistoryScript).With(x => x.Package, connectorFunctionHistoryPackage).OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.GetByIdWithInputs(It.IsAny<Guid>())).ReturnsAsync(connectorFunctionHistory);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionHistoryRepository.Update(It.IsAny<ConnectorFunctionHistory>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
			_mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BuildConnectorFunctionMessage>(), default), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connectorFunctionHistory);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new UpdateConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var command = _fixture.Build<UpdateConnectorFunctionHistoryCommand>().With(x => x.Script, It.IsAny<byte[]>()).With(x => x.Package, It.IsAny<byte[]>()).Create();
			var connectorFunction = _fixture.Build<ConnectorFunctionHistory>().With(x => x.Script, It.IsAny<byte[]>()).With(x => x.Package, It.IsAny<byte[]>).OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.GetByIdWithInputs(It.IsAny<Guid>())).ReturnsAsync(connectorFunction);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionHistoryRepository.Update(It.IsAny<ConnectorFunctionHistory>()));
			_mockUnitOfWork.Verify(x => x.Commit());
			_mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BuildConnectorFunctionMessage>(), default));

			result.Should().BeOfType<SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connectorFunction);
		}
	}
}
