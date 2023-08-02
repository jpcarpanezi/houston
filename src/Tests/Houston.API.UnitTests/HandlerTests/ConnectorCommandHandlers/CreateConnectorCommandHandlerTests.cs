using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Create;

namespace Houston.API.UnitTests.HandlerTests.ConnectorCommandHandlers {
	[TestFixture]
	public class CreateConnectorCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private CreateConnectorCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var command = _fixture.Create<CreateConnectorCommand>();
			var connector = _fixture.Build<Connector>().OmitAutoProperties().Create();
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorRepository.Add(It.IsAny<Connector>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<Connector, ConnectorViewModel>>();
			var successResult = result as SuccessResultCommand<Connector, ConnectorViewModel>;

			successResult?.StatusCode.Should().Be(HttpStatusCode.Created);
			successResult?.Response.Should().BeSameAs(connector);
		}
	}
}
