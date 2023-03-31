using Houston.Application.CommandHandlers.ConnectorCommandHandlers;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using System.Net;

namespace Houston.API.UnitTests.ConnectorEndpoints {
	public class GetConnectorCommandHandlerTests {
		private GetConnectorCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;

		[SetUp]
		public void Setup() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_handler = new GetConnectorCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithNotFoundConnector_ReturnsNotFound() {
			// Arrange
			var command = new GetConnectorCommand(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(default(Connector));

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsOkAndObject() {
			var command = new GetConnectorCommand(It.IsAny<Guid>());
			var connector = new Connector {
				Id = It.IsAny<Guid>(),
				Name = "Test Connector",
				Description = null,
				Active = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.EqualTo(connector));
			});
		}
	}
}
