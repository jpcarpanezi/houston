using Houston.Application.CommandHandlers.UserCommandHandlers.GetAll;

namespace Houston.API.UnitTests.HandlerTests.UserCommandHandlers {
	[TestFixture]
	public class GetAllUserCommandHandlerTests {
		private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
		private readonly Fixture _fixture = new();

		[Test]
		public async Task Handle_WithValidRequest_ShouldReturnOkPaginatedObject() {
			// Arrange
			var handler = new GetAllUserCommandHandler(_mockUnitOfWork.Object);
			var command = _fixture.Create<GetAllUserCommand>();
			var users = _fixture.Build<User>().OmitAutoProperties().CreateMany().ToList();
			_mockUnitOfWork.Setup(x => x.UserRepository.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(users);
			_mockUnitOfWork.Setup(x => x.UserRepository.Count()).ReturnsAsync(It.IsAny<long>());

			// Act
			var result = await handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<PaginatedResultCommand<User, UserViewModel>>();

			var paginatedResult = result as PaginatedResultCommand<User, UserViewModel>;
			paginatedResult?.StatusCode.Should().Be(HttpStatusCode.OK);
			paginatedResult?.Count.Should().Be(It.IsAny<long>());
			paginatedResult?.Response.Should().BeSameAs(users);
			paginatedResult?.PageIndex.Should().Be(command.PageIndex);
			paginatedResult?.PageSize.Should().Be(command.PageSize);
		}
	}
}
