using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class TogglePipelineStatusCommandHandler : IRequestHandler<TogglePipelineStatusCommand, ResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public TogglePipelineStatusCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand> Handle(TogglePipelineStatusCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return new ResultCommand(HttpStatusCode.NotFound, "The requested pipeline could not be found.", "pipelineNotFound");
			}

			if (pipeline.Status == Core.Enums.PipelineStatusEnum.Running) {
				var avg = await _unitOfWork.PipelineLogsRepository.DurationAverage(request.Id);
				return new ResultCommand(HttpStatusCode.Locked, DateTime.UtcNow.AddTicks((long)avg).ToString("yyyy-MM-ddTHH:mm:ssZ"));
			}

			pipeline.Status = pipeline.Status == Core.Enums.PipelineStatusEnum.Stopped
				? Core.Enums.PipelineStatusEnum.Awaiting
				: Core.Enums.PipelineStatusEnum.Stopped;
			pipeline.UpdatedBy = _claims.Id;
			pipeline.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();

			return new ResultCommand(HttpStatusCode.NoContent);
		}
	}
}
