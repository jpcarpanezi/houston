namespace Houston.API.Filters {
	public class ValidationExceptionFilter : IExceptionFilter {
		public void OnException(ExceptionContext context) {
			if (context.Exception is ValidationException ex) {
				var validationResult = new ValidationResult(ex.Errors);
				validationResult.AddToModelState(context.ModelState, null);
				context.Result = new ObjectResult(validationResult) {
					StatusCode = (int)HttpStatusCode.BadRequest
				};
				context.ExceptionHandled = true;
			}
		}
	}
}
