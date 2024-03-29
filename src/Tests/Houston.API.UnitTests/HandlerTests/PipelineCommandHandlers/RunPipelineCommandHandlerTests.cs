﻿using Houston.Application.CommandHandlers.PipelineCommandHandlers.Run;
using MassTransit;

namespace Houston.API.UnitTests.HandlerTests.PipelineCommandHandlers {
	[TestFixture]
	public class RunPipelineCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Mock<IPublishEndpoint> _mockEventBus = new();
		private readonly Mock<IUserClaimsService> _mockClaims = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithPipelineNotFound_ShouldReturnNotFoundObject() {
			// Arrange
			var handler = new RunPipelineCommandHandler(_mockUnitOfWork.Object, _mockEventBus.Object, _mockClaims.Object);
			var command = _fixture.Create<RunPipelineCommand>();
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
		public async Task Handle_WithPipelineRunning_ShouldReturnLockedObject() {
			// Arrange
			var handler = new RunPipelineCommandHandler(_mockUnitOfWork.Object, _mockEventBus.Object, _mockClaims.Object);
			var command = _fixture.Create<RunPipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().With(x => x.Status, Core.Enums.PipelineStatus.Running).Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockUnitOfWork.Setup(x => x.PipelineLogsRepository.DurationAverage(It.IsAny<Guid>(), default)).ReturnsAsync(It.IsAny<double>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.Locked);
			errorResult?.ErrorMessage.Should().BeEmpty();
			errorResult?.ErrorCode.Should().BeNull();
			errorResult?.CustomBody.Should().BeOfType<LockedMessageViewModel>();

			var customBody = errorResult?.CustomBody as LockedMessageViewModel;
			customBody?.Message.Should().Be("Server is processing a request from this pipeline. Please try again later.");
			customBody?.ErrorCode.Should().Be("pipelineRunning");
			customBody?.EstimatedCompletionTime.Should().BeCloseTo(DateTime.UtcNow.AddTicks((long)It.IsAny<double>()), TimeSpan.FromSeconds(1));
		}

		[Test]
		public async Task Handle_WithEventBusException_ShouldReturnInternalServerErrorObject() {
			// Arrange
			var handler = new RunPipelineCommandHandler(_mockUnitOfWork.Object, _mockEventBus.Object, _mockClaims.Object);
			var command = _fixture.Create<RunPipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().With(x => x.Status, Core.Enums.PipelineStatus.Awaiting).Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockEventBus.Setup(x => x.Publish(It.IsAny<RunPipelineMessage>(), default)).Throws(new Exception());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<ErrorResultCommand>();

			var errorResult = result as ErrorResultCommand;
			errorResult?.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
			errorResult?.ErrorMessage.Should().Be("Error while trying to run the pipeline.");
			errorResult?.ErrorCode.Should().Be("cannotRunPipeline");
			errorResult?.CustomBody.Should().BeNull();
		}

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnNoContentObject() {
			// Arrange
			var handler = new RunPipelineCommandHandler(_mockUnitOfWork.Object, _mockEventBus.Object, _mockClaims.Object);
			var command = _fixture.Create<RunPipelineCommand>();
			var pipeline = _fixture.Build<Pipeline>().OmitAutoProperties().With(x => x.Status, Core.Enums.PipelineStatus.Awaiting).Create();
			_mockUnitOfWork.Setup(x => x.PipelineRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(pipeline);
			_mockClaims.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockEventBus.Invocations.Clear(); // Remove invocation from previous test
			_mockEventBus.Setup(x => x.Publish(It.IsAny<RunPipelineMessage>(), default));

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			_mockEventBus.Verify(x => x.Publish(It.IsAny<RunPipelineMessage>(), default), Times.Once);

			result.Should().BeOfType<SuccessResultCommand>();

			var successResult = result as SuccessResultCommand;
			successResult?.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
