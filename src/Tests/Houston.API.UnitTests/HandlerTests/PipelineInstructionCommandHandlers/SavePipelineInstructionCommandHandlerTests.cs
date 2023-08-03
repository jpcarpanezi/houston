using Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.Save;

namespace Houston.API.UnitTests.HandlerTests.PipelineInstructionCommandHandlers {
	[TestFixture]
	public class SavePipelineInstructionCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();
		private SavePipelineInstructionCommandHandler _handler;

		[SetUp]
		public void SetUp() {
			_handler = new SavePipelineInstructionCommandHandler(_mockUnitOfWork.Object, _mockClaims.Object);
		}

		[Test]
		public async Task Handle_WhenViolatesForeignKey_ShouldReturnConflictObject() {
			// Arrange
			var command = _fixture.Create<SavePipelineInstructionCommand>();
			var databasePipelineInstructions = _fixture.Build<PipelineInstruction>().OmitAutoProperties().CreateMany().ToList();
			var databaseConnectorFunctions = _fixture.Build<ConnectorFunction>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetByIdList(It.IsAny<List<Guid>>())).ReturnsAsync(databaseConnectorFunctions);
			_mockUnitOfWork.Setup(x => x.PipelineInstructionRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(databasePipelineInstructions);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var successResult = result as ErrorResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Conflict);
			successResult?.ErrorMessage.Should().Be("Could not complete request due to a foreign key constraint violation.");
			successResult?.ErrorCode.Should().Be("foreignKeyViolation");
			successResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithInvalidConnectorFunctionInputs_ShouldReturnForbiddenObject() {
			// Arrange
			Guid connectorFunctionId = Guid.NewGuid();
			var pipelineInstructions = _fixture.Build<SavePipelineInstruction>()
									  .With(x => x.ConnectorFunctionId, connectorFunctionId)
									  .With(x => x.Inputs, new Dictionary<Guid, string?>())
									  .CreateMany(1)
									  .ToList();
			var command = _fixture.Build<SavePipelineInstructionCommand>().With(x => x.PipelineInstructions, pipelineInstructions).Create();

			var databasePipelineInstructions = _fixture.Build<PipelineInstruction>().OmitAutoProperties().CreateMany().ToList();
			var connectorFunctionInputs = _fixture.Build<ConnectorFunctionInput>().OmitAutoProperties().CreateMany(1).ToList();
			var databaseConnectorFunctions = _fixture.Build<ConnectorFunction>()
											.OmitAutoProperties()
											.With(x => x.ConnectorFunctionInputs, connectorFunctionInputs)
											.With(x => x.Id, connectorFunctionId)
											.CreateMany(1)
											.ToList();

			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetByIdList(It.IsAny<List<Guid>>())).ReturnsAsync(databaseConnectorFunctions);
			_mockUnitOfWork.Setup(x => x.PipelineInstructionRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(databasePipelineInstructions);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var successResult = result as ErrorResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Forbidden);
			successResult?.ErrorMessage.Should().Be("The number of inserted inputs is not the same as the connector function.");
			successResult?.ErrorCode.Should().Be("invalidConnectorFunctionInputs");
			successResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnCreatedObject() {
			// Arrange
			var command = _fixture.Build<SavePipelineInstructionCommand>().With(x => x.PipelineInstructions, new List<SavePipelineInstruction>()).Create();
			var databasePipelineInstructions = _fixture.Build<PipelineInstruction>().OmitAutoProperties().CreateMany().ToList();
			var databaseConnectorFunctions = _fixture.Build<ConnectorFunction>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.ConnectorFunctionRepository.GetByIdList(It.IsAny<List<Guid>>())).ReturnsAsync(databaseConnectorFunctions);
			_mockUnitOfWork.Setup(x => x.PipelineInstructionRepository.GetByPipelineId(It.IsAny<Guid>())).ReturnsAsync(databasePipelineInstructions);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			_mockUnitOfWork.Verify(x => x.PipelineInstructionRepository.RemoveRange(It.IsAny<List<PipelineInstruction>>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.PipelineInstructionRepository.AddRange(It.IsAny<List<PipelineInstruction>>()), Times.Once);
			_mockUnitOfWork.Verify(x => x.Commit(), Times.Once);

			result.Should().BeOfType<SuccessResultCommand<List<PipelineInstruction>, List<PipelineInstructionViewModel>>>();

			var successResult = result as SuccessResultCommand<List<PipelineInstruction>, List<PipelineInstructionViewModel>>;
			successResult?.StatusCode.Should().Be(HttpStatusCode.Created);
			successResult?.Response.Should().BeEmpty();
		}
	}
}
