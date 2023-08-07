using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Delete;

namespace Houston.API.UnitTests.HandlerTests.ConnectorCommandHandlers {
	[TestFixture]
	public class DeleteConnectorCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithNotFoundConnector_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new DeleteConnectorCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<DeleteConnectorCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Connector?)null);

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
		public async Task Handle_WithInactiveConnector_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new DeleteConnectorCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<DeleteConnectorCommand>();
			var connector = _fixture.Build<Connector>()
							   .OmitAutoProperties()
							   .With(x => x.Active, false)
							   .Create();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(connector);

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
		public async Task Handle_WithValidRequest_ShouldReturnNoContent() {
			// Arrange
			var handler = new DeleteConnectorCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<DeleteConnectorCommand>();
			var connector = _fixture.Build<Connector>()
							   .OmitAutoProperties()
							   .With(x => x.Active, true)
							   .Create();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorRepository.Update(It.IsAny<Connector>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
