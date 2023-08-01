namespace Houston.Application.CommandHandlers.UserCommandHandlers.GetAll {
	public sealed record GetAllUserCommand(int PageSize, int PageIndex) : IRequest<IResultCommand>;
}
