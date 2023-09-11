using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create;

namespace Houston.API.UnitTests.HandlerTests.ConnectorFunctionCommandHandlers {
	[TestFixture]
	public class CreateConnectorFunctionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Mock<IPublishEndpoint> _mockPublishEndpoint = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var handler = new CreateConnectorFunctionCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object, _mockPublishEndpoint.Object);
			var request = _fixture.Create<CreateConnectorFunctionCommand>();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository).Returns(Mock.Of<IConnectorFunctionRepository>);
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionInputRepository).Returns(Mock.Of<IConnectorFunctionInputRepository>);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(request, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionRepository.Add(It.IsAny<ConnectorFunction>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.ConnectorFunctionInputRepository.AddRange(It.IsAny<List<ConnectorFunctionInput>>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
			_mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BuildConnectorFunctionMessage>(), default), Times.Once);
		}
	}
}
