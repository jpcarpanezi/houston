using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Get;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Get;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionCommandHandlers {
	[TestFixture]
	public class GetConnectorFunctionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();
		private GetConnectorFunctionCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new GetConnectorFunctionCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithNotFoundConnectorFunction_ShouldReturnNotFoundObject() {
			// Arrange
			var command = _fixture.Create<GetConnectorFunctionCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync((ConnectorFunction?)null);

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
		public async Task Handle_WithValidConnectorFunction_ShouldReturnOkObject() {
			// Arrange
			var command = _fixture.Create<GetConnectorFunctionCommand>();
			var connector = _fixture.Build<ConnectorFunction>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<SuccessResultCommand<ConnectorFunction, ConnectorFunctionViewModel>>();

			var successResult = result as SuccessResultCommand<ConnectorFunction, ConnectorFunctionViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connector);
		}
	}
}
