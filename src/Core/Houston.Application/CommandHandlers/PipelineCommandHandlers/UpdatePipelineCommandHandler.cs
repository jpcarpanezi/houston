using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class UpdatePipelineCommandHandler : IRequestHandler<UpdatePipelineCommand, ResultCommand<Pipeline>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePipelineCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<Pipeline>> Handle(UpdatePipelineCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return new ResultCommand<Pipeline>(HttpStatusCode.NotFound, "The requested pipeline could not be found.", "pipelineNotFound", null);
			}

			pipeline.Name = request.Name;
			pipeline.Description = request.Description;
			pipeline.UpdatedBy = _claims.Id;
			pipeline.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();

			return new ResultCommand<Pipeline>(HttpStatusCode.OK, null, null, pipeline);
		}
	}
}
