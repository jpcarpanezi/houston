using System.Net;

namespace Houston.Core.Commands {
	public class ResultCommand<T> where T : class {
		public HttpStatusCode StatusCode { get; set; }

		public string? ErrorMessage { get; set; }

		public string? ErrorCode { get; set; }

		public T? Response { get; set; }

		public ResultCommand(HttpStatusCode statusCode, string? message = null, string? errorCode = null, T? response = null) {
			StatusCode = statusCode;
			ErrorMessage = message;
			ErrorCode = errorCode;
			Response = response;
		}
	}

	public class ResultCommand {
		public HttpStatusCode StatusCode { get; set; }

		public string? ErrorMessage { get; set; }

		public string? ErrorCode { get; set; }

		public ResultCommand(HttpStatusCode statusCode, string? errorMessage = null, string? errorCode = null) {
			StatusCode = statusCode;
			ErrorMessage = errorMessage;
			ErrorCode = errorCode;
		}
	}
}