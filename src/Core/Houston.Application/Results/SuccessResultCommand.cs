namespace Houston.Application.Results {
	public sealed class SuccessResultCommand : BaseResultCommand<object, object> {
		public SuccessResultCommand(HttpStatusCode statusCode = HttpStatusCode.NoContent) : base(statusCode) { }
	}

	public sealed class SuccessResultCommand<TEntity, TDto> : BaseResultCommand<TEntity, TDto> where TEntity : class where TDto : class {
		public TEntity Response { get => ResponseEntity; }

		public SuccessResultCommand(TEntity response, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode, response: response) { }
	}
}
