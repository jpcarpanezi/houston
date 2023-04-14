using Houston.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;

namespace Houston.API.Filters {
	public class ForeignKeyExceptionFilter : IExceptionFilter {
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
