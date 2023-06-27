using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class RevealPipelineTriggerKeysCommandHandler : IRequestHandler<RevealPipelineTriggerKeysCommand, ResultCommand<PipelineTrigger>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public RevealPipelineTriggerKeysCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = userClaimsService ?? throw new ArgumentNullException(nameof(userClaimsService));
		}

		public async Task<ResultCommand<PipelineTrigger>> Handle(RevealPipelineTriggerKeysCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			if (pipelineTrigger is null) {
				return new ResultCommand<PipelineTrigger>(HttpStatusCode.NotFound, "The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			if (pipelineTrigger.KeyRevealed) {
				return new ResultCommand<PipelineTrigger>(HttpStatusCode.Forbidden, "The deploy keys were already revealed.", "deployKeysRevealed");
			}

			pipelineTrigger.KeyRevealed = true;
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await _unitOfWork.Commit();

			return new ResultCommand<PipelineTrigger>(HttpStatusCode.OK, null, null, pipelineTrigger);
		}
	}
}
