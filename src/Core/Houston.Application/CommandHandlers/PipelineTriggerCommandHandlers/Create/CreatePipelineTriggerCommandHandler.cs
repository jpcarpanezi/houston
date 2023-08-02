namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public class CreatePipelineTriggerCommandHandler : IRequestHandler<CreatePipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreatePipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreatePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var anyPipelineTrigger = await _unitOfWork.PipelineTriggerRepository.AnyPipelineTrigger(request.PipelineId);
			if (anyPipelineTrigger) {
				return ResultCommand.Forbidden("The request pipeline already has a trigger.", "alreadyRegistered");
			}

			Guid pipelineTriggerId = Guid.NewGuid();
			var deployKeys = DeployKeysService.Create($"houston-{pipelineTriggerId}");
			var PipelineTriggerEvents = new List<PipelineTriggerEvent>();

			foreach (var @event in request.Events) {
				Guid pipelineTriggerEventId = Guid.NewGuid();

				var pipelineTriggerEvent = new PipelineTriggerEvent {
					Id = pipelineTriggerEventId,
					PipelineTriggerId = pipelineTriggerId,
					TriggerEventId = @event.TriggerEventId,
					PipelineTriggerFilters = @event.EventFilters.Select(x => new PipelineTriggerFilter {
						Id = Guid.NewGuid(),
						PipelineTriggerEventId = pipelineTriggerEventId,
						TriggerFilterId = x.TriggerFilterId,
						FilterValues = x.FilterValues
					}).ToList()
				};

				PipelineTriggerEvents.Add(pipelineTriggerEvent);
			}

			var pipelineTrigger = new PipelineTrigger {
				Id = pipelineTriggerId,
				PipelineId = request.PipelineId,
				SourceGit = request.SourceGit,
				PrivateKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PrivateKey)),
				PublicKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PublicKey)),
				KeyRevealed = false,
				Secret = PasswordService.HashPassword(request.Secret),
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow,
				PipelineTriggerEvents = PipelineTriggerEvents
			};

			_unitOfWork.PipelineTriggerRepository.Add(pipelineTrigger);
			await _unitOfWork.Commit();

			var response = await _unitOfWork.PipelineTriggerRepository.GetByIdWithInverseProperties(pipelineTriggerId);

			return ResultCommand.Created<PipelineTrigger, PipelineTriggerViewModel>(response!);
		}
	}
}
