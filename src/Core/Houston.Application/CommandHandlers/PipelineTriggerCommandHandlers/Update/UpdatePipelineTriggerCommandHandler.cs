namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public class UpdatePipelineTriggerCommandHandler : IRequestHandler<UpdatePipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork __unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			__unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdatePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await __unitOfWork.PipelineTriggerRepository.GetByIdWithInverseProperties(request.PipelineTriggerId);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			var pipelineTriggerEvents = new List<PipelineTriggerEvent>();

			foreach (var @event in request.Events) {
				Guid pipelineTriggerEventId = Guid.NewGuid();

				var pipelineTriggerEvent = new PipelineTriggerEvent {
					Id = pipelineTriggerEventId,
					PipelineTriggerId = request.PipelineTriggerId,
					TriggerEventId = @event.TriggerEventId,
					PipelineTriggerFilters = @event.EventFilters.Select(x => new PipelineTriggerFilter {
						Id = Guid.NewGuid(),
						PipelineTriggerEventId = pipelineTriggerEventId,
						TriggerFilterId = x.TriggerFilterId,
						FilterValues = x.FilterValues
					}).ToList()
				};

				pipelineTriggerEvents.Add(pipelineTriggerEvent);
			}

			pipelineTrigger.SourceGit = request.SourceGit;
			pipelineTrigger.PipelineTriggerEvents = pipelineTriggerEvents;
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			__unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await __unitOfWork.Commit();

			return ResultCommand.Ok<PipelineTrigger, PipelineTriggerViewModel>(pipelineTrigger);
		}
	}
}
