using System.Net;

namespace Houston.Core.Commands {
	public class ResultCommand<T> where T : class {
		public HttpStatusCode StatusCode { get; set; }

		public string? ErrorMessage { get; set; }

		public T? Response { get; set; }

		public ResultCommand(HttpStatusCode statusCode, string? message, T? response) {
			StatusCode = statusCode;
			ErrorMessage = message;
			Response = response;
		}
	}

	public class ResultCommand {
		public HttpStatusCode StatusCode { get; set; }

		public string? ErrorMessage { get; set; }

		public ResultCommand(HttpStatusCode statusCode, string? errorMessage = null) {
			StatusCode = statusCode;
			ErrorMessage = errorMessage;
		}
	}
}