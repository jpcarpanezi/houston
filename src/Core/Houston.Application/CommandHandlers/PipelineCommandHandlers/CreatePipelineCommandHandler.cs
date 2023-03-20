using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class CreatePipelineCommandHandler : IRequestHandler<CreatePipelineCommand, ResultCommand<Pipeline>> {
		private readonly IUnitOfWork _unitOfWork;

		public CreatePipelineCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<Pipeline>> Handle(CreatePipelineCommand request, CancellationToken cancellationToken) {
			ObjectId userId = ObjectId.Parse("6406274256017a23e89a7dd6");
			Pipeline pipeline = new() {
				Id = ObjectId.GenerateNewId(),
				Name = request.Name,
				Description = request.Description,
				PipelineStatus = Core.Enums.PipelineStatusEnum.Awaiting,
				RepositoryHostConfig = request.RepositoryHostConfig,
				SourceCode = request.SourceCode,
				DeployKey = request.DeployKey,
				Secret = request.Secret,
				LastRun = null,
				Triggers = request.Triggers,
				Instructions =  new List<PipelineInstruction>(),
				CreatedBy = userId,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = userId,
				LastUpdate = DateTime.UtcNow
			};

			await _unitOfWork.PipelineRepository.InsertOneAsync(pipeline);

			return new ResultCommand<Pipeline>(HttpStatusCode.Created, null, pipeline);
		}
	}
}
