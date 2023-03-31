using Houston.Application.CommandHandlers.ConnectorCommandHandlers;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using System.Net;

namespace Houston.API.UnitTests.ConnectorCommandHandlers {
	public class UpdateConnectorCommandHandlerTests {
		private UpdateConnectorCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new UpdateConnectorCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		public async Task Handle_WithNotFoundConnector_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new UpdateConnectorCommand(It.IsAny<Guid>(), "Test Connector", "Test Connector Description");
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(default(Connector));

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidConnector"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithInactiveConnector_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new UpdateConnectorCommand(It.IsAny<Guid>(), "Test Connector", "Test Connector Description");
			var connector = new Connector { 
				Id = It.IsAny<Guid>(),
				Name = "Connector Test",
				Description = null,
				Active = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
				Assert.That(result.ErrorMessage, Is.EqualTo("invalidConnector"));
				Assert.That(result.Response, Is.Null);
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsOkAndObject() {
			// Arrange
			var command = new UpdateConnectorCommand(It.IsAny<Guid>(), "Test Connector", "Test Connector Description");
			var connector = new Connector {
				Id = It.IsAny<Guid>(),
				Name = "Connector Test",
				Description = null,
				Active = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.EqualTo(connector));
			});
			_mockUnitOfWork.Verify(x => x.ConnectorRepository.Update(It.IsAny<Connector>()));
		}
	}
}
