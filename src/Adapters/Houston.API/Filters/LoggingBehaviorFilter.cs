namespace Houston.API.Filters {
	public class LoggingBehaviorFilter<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			if (Log.IsEnabled(LogEventLevel.Debug)) {
				Log.Debug("Handling {requestName}", typeof(TRequest).Name);

				Type myType = request.GetType();
				IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
				foreach (PropertyInfo prop in props) {
					object? propValue = prop.GetValue(request, null);
					Log.Debug("{propName}: {propValue}", prop.Name, propValue);
				}

				var response = await next();

				Log.Debug("Handled {responseName}", typeof(TResponse).Name);
				return response;
			} else {
				return await next();
			}
		}
	}
}
