namespace Houston.API.Filters {
	public class ForeignKeyExceptionFilter : Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter {
		public void OnException(ExceptionContext context) {
			if (context.Exception is DbUpdateException ex && ex.InnerException is NpgsqlException npgsqlException && npgsqlException.SqlState == "23503") {
				context.Result = new ObjectResult(new MessageViewModel("Could not complete request due to a foreign key constraint violation.", "foreingKeyViolation")) {
					StatusCode = (int)HttpStatusCode.Conflict
				};

				context.ExceptionHandled = true;
			}
		}
	}
}
