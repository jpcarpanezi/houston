namespace Houston.Core.Commands {
	public class PaginatedResultCommand<T> where T : class {
		public IEnumerable<T> Response { get; private set; }

		public int PageSize { get; private set; }

		public int PageIndex { get; private set; }

		public long Count { get; private set; }

		public PaginatedResultCommand(IEnumerable<T> response, int pageSize, int pageIndex, long count) {
			Response = response;
			PageSize = pageSize;
			PageIndex = pageIndex;
			Count = count;
		}
	}
}
