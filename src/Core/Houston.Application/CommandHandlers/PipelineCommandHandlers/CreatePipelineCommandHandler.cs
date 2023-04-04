using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class CreatePipelineCommandHandler : IRequestHandler<CreatePipelineCommand, ResultCommand<Pipeline>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreatePipelineCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<Pipeline>> Handle(CreatePipelineCommand request, CancellationToken cancellationToken) {
			Guid pipelineId = Guid.NewGuid();
			var pipeline = new Pipeline {
				Id = pipelineId,
				Name = request.Name,
				Description = request.Description,
				Active = true,
				Status = Core.Enums.PipelineStatusEnum.Awaiting,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.PipelineRepository.Add(pipeline);
			await _unitOfWork.Commit();

			pipeline = await _unitOfWork.PipelineRepository.GetActive(pipelineId);

			return new ResultCommand<Pipeline>(HttpStatusCode.Created, null, pipeline);
		}
	}
}
