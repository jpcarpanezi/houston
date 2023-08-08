using AutoMapper;
using static StackExchange.Redis.Role;

namespace Houston.Application.Results {
	public abstract class BaseResultCommand<TEntity, TDto> : IResultCommand where TEntity : class where TDto : class {
		public HttpStatusCode StatusCode { get; protected set; }

		protected string ResponseErrorMessage { get; } = string.Empty;

		protected string? ResponseErrorCode { get; }

		protected object? ResponseCustomBody { get; }

		protected TEntity ResponseEntity { get; } = default!;

		protected int ResponsePageSize { get; }

		protected int ResponsePageIndex { get; }

		protected long ResponseCount { get; }

		protected BaseResultCommand(HttpStatusCode statusCode, string errorMessage, string? errorCode = null) {
			StatusCode = statusCode;
			ResponseErrorMessage = errorMessage;
			ResponseErrorCode = errorCode;
		}

		protected BaseResultCommand(HttpStatusCode statusCode, TEntity response, int pageSize, int pageIndex, long count) {
			StatusCode = statusCode;
			ResponseEntity = response;
			ResponsePageSize = pageSize;
			ResponsePageIndex = pageIndex;
			ResponseCount = count;
		}

		protected BaseResultCommand(HttpStatusCode statusCode, object customBody) {
			StatusCode = statusCode;
			ResponseCustomBody = customBody;
		}

		protected BaseResultCommand(HttpStatusCode statusCode, TEntity response) {
			StatusCode = statusCode;
			ResponseEntity = response;
		}

		protected BaseResultCommand(HttpStatusCode statusCode) {
			StatusCode = statusCode;
		}

		public virtual async Task ExecuteResultAsync(ActionContext context) {
			var objectResult = new ObjectResult(null) {
				StatusCode = (int)StatusCode
			};

			Type responseType = GetType();

			var config = new MapperConfiguration(cfg => cfg.AddProfile<MapProfile>());
			var mapper = new Mapper(config);

			if (responseType == typeof(ErrorResultCommand)) {
				objectResult.Value = ResponseCustomBody is not null ? ResponseCustomBody : new MessageViewModel(ResponseErrorMessage, ResponseErrorCode);
			}

			if (responseType == typeof(SuccessResultCommand<TEntity, TDto>)) {
				var dto = mapper.Map<TDto>(ResponseEntity);
				objectResult.Value = dto;
			}

			if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(PaginatedResultCommand<,>)) {
				var dto = mapper.Map<List<TDto>>(ResponseEntity);
				objectResult.Value = new PaginatedItemsViewModel<TDto>(ResponsePageIndex, ResponsePageSize, ResponseCount, dto);
			}

			await objectResult.ExecuteResultAsync(context);
		}
	}
}
