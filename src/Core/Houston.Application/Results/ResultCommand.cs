namespace Houston.Application.Results {
	public abstract class ResultCommand {
		public static ErrorResultCommand Error(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.Forbidden) => new(errorMessage, statusCode);

		public static ErrorResultCommand Error(string errorMessage, string errorCode, HttpStatusCode statusCode = HttpStatusCode.Forbidden) => new(errorMessage, errorCode, statusCode);

		public static ErrorResultCommand Error(object customBody, HttpStatusCode statusCode = HttpStatusCode.Forbidden) => new(customBody, statusCode);

		public static PaginatedResultCommand<TEntity, TDto> Paginated<TEntity, TDto>(IEnumerable<TEntity> response, int pageSize, int pageIndex, long count, HttpStatusCode statusCode = HttpStatusCode.OK) where TEntity : class where TDto : class => new(response, pageSize, pageIndex, count, statusCode);

		public static SuccessResultCommand Success(HttpStatusCode statusCode = HttpStatusCode.NoContent) => new(statusCode);

		public static SuccessResultCommand<TEntity, TDto> Success<TEntity, TDto>(TEntity response, HttpStatusCode statusCode = HttpStatusCode.OK) where TEntity : class where TDto : class => new(response, statusCode);

		public static SuccessResultCommand<TEntity, TDto> Ok<TEntity, TDto>(TEntity response) where TEntity : class where TDto : class => new(response, HttpStatusCode.OK);

		public static SuccessResultCommand<TEntity, TDto> Created<TEntity, TDto>(TEntity response) where TEntity : class where TDto : class => new(response, HttpStatusCode.Created);

		public static SuccessResultCommand NoContent() => new(HttpStatusCode.NoContent);

		public static ErrorResultCommand NotFound(string errorMessage, string? errorCode) => new(errorMessage, errorCode, HttpStatusCode.NotFound);

		public static ErrorResultCommand Conflict(string errorMessage, string? errorCode) => new(errorMessage, errorCode, HttpStatusCode.Conflict);

		public static ErrorResultCommand Forbidden(string errorMessage, string? errorCode) => new(errorMessage, errorCode, HttpStatusCode.Forbidden);

		public static ErrorResultCommand Locked(object customBody) => new(customBody, HttpStatusCode.Locked);

		public static ErrorResultCommand InternalServerError(string errorMessage, string? errorCode) => new(errorMessage, errorCode, HttpStatusCode.InternalServerError);
	}
}
