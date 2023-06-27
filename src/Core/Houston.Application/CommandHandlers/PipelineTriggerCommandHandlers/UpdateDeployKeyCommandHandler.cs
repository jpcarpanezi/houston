using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Infrastructure.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class UpdateDeployKeyCommandHandler : IRequestHandler<UpdateDeployKeyCommand, ResultCommand<PipelineTrigger>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdateDeployKeyCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<PipelineTrigger>> Handle(UpdateDeployKeyCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			if (pipelineTrigger is null) {
				return new ResultCommand<PipelineTrigger>(HttpStatusCode.NotFound, "The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			var deployKeys = DeployKeysService.Create($"houston-{pipelineTrigger.Id}");
			pipelineTrigger.PrivateKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PrivateKey));
			pipelineTrigger.PublicKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PublicKey));
			pipelineTrigger.KeyRevealed = false;
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await _unitOfWork.Commit();

			return new ResultCommand<PipelineTrigger>(HttpStatusCode.NoContent);
		}
	}
}
