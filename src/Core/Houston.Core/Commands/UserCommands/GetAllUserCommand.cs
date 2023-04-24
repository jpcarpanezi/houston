using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class GetAllUserCommand : IRequest<PaginatedResultCommand<User>> {
		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public GetAllUserCommand() { }

		public GetAllUserCommand(int pageSize, int pageIndex) {
			PageSize = pageSize;
			PageIndex = pageIndex;
		}
	}
}
