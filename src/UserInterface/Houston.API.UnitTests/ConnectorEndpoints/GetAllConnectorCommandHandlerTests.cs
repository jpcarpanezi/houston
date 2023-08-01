using Houston.Application.CommandHandlers.ConnectorCommandHandlers.GetAll;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;

namespace Houston.API.UnitTests.ConnectorEndpoints
{
    public class GetAllConnectorCommandHandlerTests {
		private GetAllConnectorCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_handler = new GetAllConnectorCommandHandler(_mockUnitOfWork.Object);
		}

		[Test]
		public async Task Handle_WithNoValues_ReturnsOkAndPaginatedItemsViewModel() {
			// Arrange
			int pageIndex = 0;
			int pageSize = 10;
			var command = new GetAllConnectorCommand(pageSize, pageIndex);
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.CountActives()).ReturnsAsync(0);
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetAllActives(pageSize, pageIndex)).ReturnsAsync(new List<Connector>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.Count, Is.EqualTo(0));
				Assert.That(result.PageIndex, Is.EqualTo(pageIndex));
				Assert.That(result.PageSize, Is.EqualTo(pageSize));
				Assert.That(result.Response, Is.Empty);
			});
		}

		[Test]
		public async Task Handle_WithValues_ReturnsOkAndPaginatedItemsViewModel() {
			// Arrange
			int pageIndex = 0;
			int pageSize = 10;
			var command = new GetAllConnectorCommand(pageSize, pageIndex);
			var connectors = new List<Connector> {
				new Connector {
					Id = It.IsAny<Guid>(),
					Name = "Test Connector",
					Description = null,
					Active = true,
					CreatedBy = It.IsAny<Guid>(),
					CreationDate = It.IsAny<DateTime>(),
					UpdatedBy = It.IsAny<Guid>(),
					LastUpdate = It.IsAny<DateTime>()
				}
			};
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.CountActives()).ReturnsAsync(1);
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetAllActives(pageSize, pageIndex)).ReturnsAsync(connectors);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.Count, Is.EqualTo(1));
				Assert.That(result.PageIndex, Is.EqualTo(pageIndex));
				Assert.That(result.PageSize, Is.EqualTo(pageSize));
				Assert.That(result.Response, Is.EqualTo(connectors));
			});
		}
	}
}
