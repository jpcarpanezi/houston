namespace Houston.Application.PipelineBehaviors {
	public class ValidationPreProcessorFilter<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull {
		public readonly IValidator<TRequest> _validator;

		public ValidationPreProcessorFilter(IValidator<TRequest> validator) {
			_validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		public async Task Process(TRequest request, CancellationToken cancellationToken) {
			await _validator.ValidateAndThrowAsync(request, cancellationToken);
		}
	}
}
