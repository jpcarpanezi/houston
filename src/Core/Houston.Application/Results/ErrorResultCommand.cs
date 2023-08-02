namespace Houston.Application.Results {
	public sealed class ErrorResultCommand : BaseResultCommand<object, object> {
		public string ErrorMessage { get => ResponseErrorMessage; }

		public string? ErrorCode { get => ResponseErrorCode; }

		public object? CustomBody { get => ResponseCustomBody; }

		public ErrorResultCommand(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.Forbidden) : base(statusCode, errorMessage) { }

		public ErrorResultCommand(string errorMessage, string? errorCode, HttpStatusCode statusCode = HttpStatusCode.Forbidden) : base(statusCode, errorMessage, errorCode) { }

		public ErrorResultCommand(object customBody, HttpStatusCode statusCode = HttpStatusCode.Forbidden) : base(statusCode, customBody) { }
	}
}
