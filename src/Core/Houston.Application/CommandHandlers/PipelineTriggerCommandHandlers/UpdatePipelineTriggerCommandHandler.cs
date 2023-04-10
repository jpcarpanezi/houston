using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using Houston.Core.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class UpdatePipelineTriggerCommandHandler : IRequestHandler<UpdatePipelineTriggerCommand, ResultCommand<PipelineTrigger>> {
		private readonly IUnitOfWork __unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			__unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<PipelineTrigger>> Handle(UpdatePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await __unitOfWork.PipelineTriggerRepository.GetByIdWithInverseProperties(request.PipelineTriggerId);
			if (pipelineTrigger is null) {
				return new ResultCommand<PipelineTrigger>(HttpStatusCode.Forbidden, "invalidPipelineTrigger", null);
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

			return new ResultCommand<PipelineTrigger>(HttpStatusCode.OK, null, pipelineTrigger);
		}
	}
}
