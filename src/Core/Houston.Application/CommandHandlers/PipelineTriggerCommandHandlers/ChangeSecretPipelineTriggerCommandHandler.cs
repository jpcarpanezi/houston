using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class ChangeSecretPipelineTriggerCommandHandler : IRequestHandler<ChangeSecretPipelineTriggerCommand, ResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public ChangeSecretPipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand> Handle(ChangeSecretPipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByIdAsync(request.PipelineTriggerId);
			if (pipelineTrigger is null) {
				return new ResultCommand(HttpStatusCode.NotFound, "The requested pipeline trigger could not be found.");
			}

			if (!PasswordService.IsPasswordStrong(request.NewSecret)) {
				return new ResultCommand(HttpStatusCode.BadRequest, "weakPassword");
			}

			pipelineTrigger.Secret = PasswordService.HashPassword(request.NewSecret);
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await _unitOfWork.Commit();

			return new ResultCommand(HttpStatusCode.NoContent);
		}
	}
}
