using Houston.Application.CommandHandlers.ConnectorCommandHandlers;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using System.Net;

namespace Houston.API.UnitTests.ConnectorCommandHandlers {
	public class CreateConnectorCommandHandlerTests {
		private CreateConnectorCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		public async Task Handle_WithValidParameters_ReturnsOk() {
			// Assert
			var command = new CreateConnectorCommand("Test Connector", "Test Description Connector");
			var connector = new Connector {
				Id = It.IsAny<Guid>(),
				Name = "Test Connector",
				Description = "Test Description Connector",
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUserClaimsService.Setup(x => x.Id).Returns(It.IsAny<Guid>());
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdWithInverseProperties(It.IsAny<Guid>())).ReturnsAsync(connector);

			// Act
			var result = await _handler.Handle(command, default);

			// Arrange
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.EqualTo(connector));
			});
			_mockUnitOfWork.Verify(x => x.ConnectorRepository.Add(It.IsAny<Connector>()), Times.Once);
		}
	}
}
