using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class UpdatePipelineInstructionsCommandHandler : IRequestHandler<UpdatePipelineInstructionsCommand, ResultCommand<List<PipelineInstruction>>> {
		private readonly IUnitOfWork _unitOfWork;

		public UpdatePipelineInstructionsCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<List<PipelineInstruction>>> Handle(UpdatePipelineInstructionsCommand request, CancellationToken cancellationToken) {
			ObjectId userId = ObjectId.Parse("6406274256017a23e89a7dd6");
			List<PipelineInstruction> pipelineInstructions = new();
			foreach (var instruction in request.PipelineInstructions) {
				PipelineInstruction pipelineInstruction = new() {
					Id = ObjectId.GenerateNewId(),
					ConnectorFunctionId = instruction.ConnectorFunctionId,
					Comments = instruction.Comments,
					InterfaceId = instruction.InterfaceId,
					Connections = instruction.Connections,
					PosX = instruction.PosX,
					PosY = instruction.PosY,
					Inputs = instruction.Inputs,
					Script = instruction.Script,
					CreatedBy = userId,
					CreationDate = DateTime.UtcNow,
					UpdatedBy = userId,
					LastUpdate = DateTime.UtcNow
				};

				pipelineInstructions.Add(pipelineInstruction);
			};

			await _unitOfWork.PipelineRepository.UpdateOneAsync(x => x.Id, request.PipelineId, x => x.Instructions, pipelineInstructions);

			return new ResultCommand<List<PipelineInstruction>>(HttpStatusCode.OK, null, pipelineInstructions);
		}
	}
}
