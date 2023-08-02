using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Update;

namespace Houston.API.UnitTests.HandlerTests.ConnectorCommandHandlers {
	[TestFixture]
	public class UpdateConnectorCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private UpdateConnectorCommandHandler _handler; 

		[SetUp]
		public void SetUp() {
			_handler = new UpdateConnectorCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WhenConnectorNotFound_ReturnsNotFoundObject() {
			// Arrange
			var command = _fixture.Create<UpdateConnectorCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync((Connector?)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WhenConnectorInactive_ReturnsNotFoundObject() {
			// Arrange
			var command = _fixture.Create<UpdateConnectorCommand>();
			var connector = _fixture.Build<Connector>().OmitAutoProperties().With(x => x.Active, false).Create();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested connector could not be found.");
			errorResult?.ErrorCode.Should().Be("connectorNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var command = _fixture.Create<UpdateConnectorCommand>();
			var connector = _fixture.Build<Connector>().OmitAutoProperties().With(x => x.Active, true).Create();
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(connector);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorRepository.Update(It.IsAny<Connector>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<Connector, ConnectorViewModel>>();

			var successResult = result as SuccessResultCommand<Connector, ConnectorViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(connector);
		}
	}
}
