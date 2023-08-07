using Houston.Application.CommandHandlers.PipelineCommandHandlers.Update;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	[TestFixture]
	public class UpdatePipelineCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithPipelineNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new UpdatePipelineCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<UpdatePipelineCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync((Pipeline?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested pipeline could not be found.");
			errorResult?.ErrorCode.Should().Be("pipelineNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new UpdatePipelineCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<UpdatePipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineRepository.Update(It.IsAny<Pipeline>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<Pipeline, PipelineViewModel>>();

			var successResult = result as SuccessResultCommand<Pipeline, PipelineViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipeline);
		}
	}
}
