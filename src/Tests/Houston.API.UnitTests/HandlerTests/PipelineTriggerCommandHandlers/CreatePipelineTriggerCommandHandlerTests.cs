using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create;

namespace Houston.API.UnitTests.HandlerTests.PipelineTriggerCommandHandlers {
	[TestFixture]
	public class CreatePipelineTriggerCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private CreatePipelineTriggerCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new CreatePipelineTriggerCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WithExistingPipelineTrigger_ShouldReturnForbiddenObject() {
			// Arrange
			var command = _fixture.Create<CreatePipelineTriggerCommand>();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.AnyPipelineTrigger(It.IsAny<Guid>())).ReturnsAsync(true);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			errorResult?.ErrorMessage.Should().Be("The request pipeline already has a trigger.");
			errorResult?.ErrorCode.Should().Be("alreadyRegistered");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var command = _fixture.Create<CreatePipelineTriggerCommand>();
			var pipelineTrigger = _fixture.Build<PipelineTrigger>().OmitAutoProperties().Create();
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.AnyPipelineTrigger(It.IsAny<Guid>())).ReturnsAsync(false);
			_mockUnitOfWork.Setup(x => x.PipelineTriggerRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(pipelineTrigger);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineTriggerRepository.Add(It.IsAny<PipelineTrigger>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<PipelineTrigger, PipelineTriggerViewModel>>();
			
			var successResult = result as SuccessResultCommand<PipelineTrigger, PipelineTriggerViewModel>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Created);
			successResult?.Response.Should().BeSameAs(pipelineTrigger);
		}
	}
}
