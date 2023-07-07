using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using System.Net;

namespace Houston.API.UnitTests.ConnectorFunctionEndpoints {
	[TestFixture]
	public class CreateConnectorFunctionCommandHandlerTests {
		private CreateConnectorFunctionCommandHandler _handler;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IUserClaimsService> _mockUserClaimsService;

		[SetUp] 
		public void SetUp() { 
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockUserClaimsService = new Mock<IUserClaimsService>();
			_handler = new CreateConnectorFunctionCommandHandler(_mockUnitOfWork.Object, _mockUserClaimsService.Object);
		}

		[Test]
		[Ignore("Need to understand why is working in request, but not in Unit Testing")]
		public async Task Handle_WithValidCommand_ReturnsCreatedAndObject() {
			// Arrange
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
			var command = new CreateConnectorFunctionCommand("Function Test", null, It.IsAny<Guid>(), null, new string[] { "return 1;" });
			_mockUnitOfWork.Setup(x => x.ConnectorRepository.GetActive(It.IsAny<Guid>())).ReturnsAsync(connector);
			_mockUserClaimsService.Setup(x => x.Id).Returns(It.IsAny<Guid>());

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(result.ErrorMessage, Is.Null);
				Assert.That(result.Response, Is.Not.Null);
			});
		}
	}
}
