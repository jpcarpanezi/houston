using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Create;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionHistoryCommandHandlers {
	public class CreateConnectorFunctionHistoryCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Mock<IPublishEndpoint> _mockEventBus = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithInactiveConnectorFunction_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new CreateConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockEventBus.Object);
			var command = _fixture.Create<CreateConnectorFunctionHistoryCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync((ConnectorFunction?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector function could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorFunctionNotFound");
		}

		[Test]
		public async Task Handle_WithExistingVersion_ShouldReturnConflictObject() {
			// Arrange
			var handler = new CreateConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockEventBus.Object);
			var command = _fixture.Create<CreateConnectorFunctionHistoryCommand>();
			var connectorFunction = _fixture.Build<ConnectorFunction>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connectorFunction);
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.VersionExists(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Conflict);
			errorResult?.ErrorMessage.Should().Be("The requested version already exists.");
			errorResult?.ErrorCode.Should().Be("versionAlreadyExists");
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var handler = new CreateConnectorFunctionHistoryCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockEventBus.Object);
			var command = _fixture.Create<CreateConnectorFunctionHistoryCommand>();
			var connectorFunction = _fixture.Build<ConnectorFunction>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connectorFunction);
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionHistoryRepository.VersionExists(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionInputRepository).Returns(Mock.Of<IConnectorFunctionInputRepository>);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionHistoryRepository.Add(It.IsAny<ConnectorFunctionHistory>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionInputRepository.AddRange(It.IsAny<List<ConnectorFunctionInput>>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
			_mockEventBus.Verify(x => x.Publish(It.IsAny<BuildConnectorFunctionMessage>(), default), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Created);
			successResult?.Response.Should().BeOfType<ConnectorFunctionHistory>();
		}
	}
}
