namespace Houston.Application.Results {
	public sealed class PaginatedResultCommand<TEntity, TDto> : BaseResultCommand<IEnumerable<TEntity>, TDto> where TEntity : class where TDto : class {
		public IEnumerable<TEntity> Response { get => ResponseEntity; }

		public long Count { get => ResponseCount; }

		public int PageSize { get => ResponsePageSize; }

		public int PageIndex { get => ResponsePageIndex; }

		public PaginatedResultCommand(IEnumerable<TEntity> response, int pageSize, int pageIndex, long count, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode, response, pageSize, pageIndex, count) { }
	}
}
