using Houston.Application.CommandHandlers.PipelineCommandHandlers.Create;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	[TestFixture]
	public class CreatePipelineCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private CreatePipelineCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new CreatePipelineCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var command = _fixture.Create<CreatePipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineRepository.Add(It.IsAny<Pipeline>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<Pipeline, PipelineViewModel>>();

			var successResult = result as SuccessResultCommand<Pipeline, PipelineViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Created);
			successResult?.Response.Should().BeSameAs(pipeline);
		}
	}
}
