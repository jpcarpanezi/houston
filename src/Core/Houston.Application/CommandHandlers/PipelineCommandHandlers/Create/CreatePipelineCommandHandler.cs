namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Create {
	public class CreatePipelineCommandHandler : IRequestHandler<CreatePipelineCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreatePipelineCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreatePipelineCommand request, CancellationToken cancellationToken) {
			Guid pipelineId = Guid.NewGuid();
			var pipeline = new Pipeline {
				Id = pipelineId,
				Name = request.Name,
				Description = request.Description,
				Active = true,
				Status = PipelineStatusEnum.Awaiting,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.PipelineRepository.Add(pipeline);
			await _unitOfWork.Commit();

			pipeline = await _unitOfWork.PipelineRepository.GetActive(pipelineId);

			return ResultCommand.Created<Pipeline, PipelineViewModel>(pipeline!);
		}
	}
}
