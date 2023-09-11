using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionCommandHandlers {
	[TestFixture]
	public class UpdateConnectorFunctionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Mock<IPublishEndpoint> _mockPublishEndpoint = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithConnectorFunctionNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new UpdateConnectorFunctionCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var command = _fixture.Create<UpdateConnectorFunctionCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetByIdWithInputs(It.IsAny<Guid>())).ReturnsAsync((ConnectorFunction?)null);

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
			var handler = new UpdateConnectorFunctionCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var commandScript = _fixture.CreateMany<byte>().ToArray();
			var commandPackage = _fixture.CreateMany<byte>().ToArray();
			var connectorFunctionScript = _fixture.CreateMany<byte>().ToArray();
			var connectorFunctionPackage = _fixture.CreateMany<byte>().ToArray();
			var command = _fixture.Build<UpdateConnectorFunctionCommand>().With(x => x.Script, commandScript).With(x => x.Package, commandPackage).Create();
			var connectorFunction = _fixture.Build<ConnectorFunction>().With(x => x.Script, connectorFunctionScript).With(x => x.Package, connectorFunctionPackage).OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetByIdWithInputs(It.IsAny<Guid>())).ReturnsAsync(connectorFunction);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionRepository.Update(It.IsAny<ConnectorFunction>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
			_mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BuildConnectorFunctionMessage>(), default), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<ConnectorFunction, ConnectorFunctionViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunction, ConnectorFunctionViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connectorFunction);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new UpdateConnectorFunctionCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var command = _fixture.Build<UpdateConnectorFunctionCommand>().With(x => x.Script, It.IsAny<byte[]>()).With(x => x.Package, It.IsAny<byte[]>()).Create();
			var connectorFunction = _fixture.Build<ConnectorFunction>().With(x => x.Script, It.IsAny<byte[]>()).With(x => x.Package, It.IsAny<byte[]>).OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetByIdWithInputs(It.IsAny<Guid>())).ReturnsAsync(connectorFunction);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionRepository.Update(It.IsAny<ConnectorFunction>()));
			_mockUnitOfWork.Verify(x => x.Commit());
			_mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BuildConnectorFunctionMessage>(), default), Times.Never);

			result.Should().BeOfType<SuccessResultCommand<ConnectorFunction, ConnectorFunctionViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunction, ConnectorFunctionViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connectorFunction);
		}
	}
}
