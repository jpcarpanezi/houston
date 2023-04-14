using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Services;
using Houston.Infrastructure.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class CreatePipelineTriggerCommandHandler : IRequestHandler<CreatePipelineTriggerCommand, ResultCommand<PipelineTrigger>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreatePipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<PipelineTrigger>> Handle(CreatePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var anyPipelineTrigger = await _unitOfWork.PipelineTriggerRepository.AnyPipelineTrigger(request.PipelineId);
			if (anyPipelineTrigger) {
				return new ResultCommand<PipelineTrigger>(HttpStatusCode.Forbidden, "The request pipeline already has a trigger.", "alreadyRegistered", null);
			}

			Guid pipelineTriggerId = Guid.NewGuid();
			var deployKeys = DeployKeysService.Create();
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
				DeployKey = deployKeys.PrivateKey,
				PublicKey = deployKeys.PublicKey,
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

			return new ResultCommand<PipelineTrigger>(HttpStatusCode.Created, null, null, response);
		}
	}
}
