using Houston.Application.CommandHandlers.ConnectorCommandHandlers;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using System.Net;

namespace Houston.API.UnitTests.ConnectorEndpoints {
	public class DeleteConnectorCommandHandlerTests {
		private DeleteConnectorCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new DeleteConnectorCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		public async Task Handle_WithNullConnector_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new DeleteConnectorCommand(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Connector));

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("connectorNotFound"));
			});
		}

		[Test]
		public async Task Handle_WithInativeConnector_ReturnsForbiddenAndErrorMessage() {
			// Arrange
			var command = new DeleteConnectorCommand(It.IsAny<Guid>());
			var connector = new Connector { 
				Id = It.IsAny<Guid>(),
				Name = "Connector Test",
				Description = "Description Test",
				Active = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
				Assert.That(result.ErrorMessage, Is.Not.Null);
				Assert.That(result.ErrorCode, Is.EqualTo("connectorNotFound"));
			});
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsNoContent() {
			// Arrange
			var command = new DeleteConnectorCommand(It.IsAny<Guid>());
			var connector = new Connector {
				Id = It.IsAny<Guid>(),
				Name = "Connector Test",
				Description = "Description Test",
				Active = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
				Assert.That(result.ErrorMessage, Is.Null);
			});
		}
	}
}
