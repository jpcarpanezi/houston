using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Delete;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionCommandHandlers {
	[TestFixture]
	public class DeleteConnectorFunctionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private DeleteConnectorFunctionCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new DeleteConnectorFunctionCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WithNotFoundConnectorFunction_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Create<DeleteConnectorFunctionCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(command.Id)).ReturnsAsync((ConnectorFunction?)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector function could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorFunctionNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContent() {
			// Arrange
			var command = _fixture.Create<DeleteConnectorFunctionCommand>();
			var connectorFunction = _fixture.Build<ConnectorFunction>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(command.Id)).ReturnsAsync(connectorFunction);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionRepository.Update(It.IsAny<ConnectorFunction>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();
			
			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
