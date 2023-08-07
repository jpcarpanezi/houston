using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.RevealKeys;

namespace Houston.API.UnitTests.HandlerTests.PipelineTriggerCommandHandlers {
	[TestFixture]
	public class RevealPipelineTriggerKeysCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithPipelineTriggerNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new RevealPipelineTriggerKeysCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<RevealPipelineTriggerKeysCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync((PipelineTrigger?)null);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.NotFound);
			errorResult?.ErrorMessage.Should().Be("The requested pipeline trigger could not be found.");
			errorResult?.ErrorCode.Should().Be("pipelineTriggerNotFound");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithAlreadyRevealedKey_ShouldReturnForbiddenObject() {
			// Arrange
			var handler = new RevealPipelineTriggerKeysCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<RevealPipelineTriggerKeysCommand>();
			var pipelineTrigger = _fixture.Build<PipelineTrigger>().OmitAutoProperties().With(x => x.KeyRevealed, true).Create();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(pipelineTrigger);

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("The deploy keys were already revealed.");
			errorResult?.ErrorCode.Should().Be("deployKeysRevealed");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkObject() {
			// Arrange
			var handler = new RevealPipelineTriggerKeysCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
			var command = _fixture.Create<RevealPipelineTriggerKeysCommand>();
			var pipelineTrigger = _fixture.Build<PipelineTrigger>().OmitAutoProperties().With(x => x.KeyRevealed, false).Create();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(pipelineTrigger);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineTriggerRepository.Update(It.IsAny<PipelineTrigger>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<PipelineTrigger, PipelineTriggerViewModel>>();

			var successResult = result as SuccessResultCommand<PipelineTrigger, PipelineTriggerViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			successResult?.Response.Should().BeSameAs(pipelineTrigger);
		}
	}
}
