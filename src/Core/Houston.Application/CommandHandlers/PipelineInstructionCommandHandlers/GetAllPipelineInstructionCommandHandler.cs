using Houston.Core.Commands;
using Houston.Core.Commands.PipelineInstructionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers {
	public class GetAllPipelineInstructionCommandHandler : IRequestHandler<GetAllPipelineInstructionCommand, ResultCommand<List<PipelineInstruction>>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllPipelineInstructionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<List<PipelineInstruction>>> Handle(GetAllPipelineInstructionCommand request, CancellationToken cancellationToken) {
			var pipelineInstructions = await _unitOfWork.PipelineInstructionRepository.GetByPipelineId(request.PipelineId);

			return new ResultCommand<List<PipelineInstruction>>(HttpStatusCode.OK, null, null, pipelineInstructions);
		}
	}
}
