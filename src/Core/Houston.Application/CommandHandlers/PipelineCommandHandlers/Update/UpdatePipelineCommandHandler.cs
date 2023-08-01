namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Update {
	public class UpdatePipelineCommandHandler : IRequestHandler<UpdatePipelineCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePipelineCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdatePipelineCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return ResultCommand.NotFound("The requested pipeline could not be found.", "pipelineNotFound");
			}

			pipeline.Name = request.Name;
			pipeline.Description = request.Description;
			pipeline.UpdatedBy = _claims.Id;
			pipeline.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();

			return ResultCommand.Ok<Pipeline, PipelineViewModel>(pipeline);
		}
	}
}
