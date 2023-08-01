using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class DeletePipelineTriggerCommandHandler : IRequestHandler<DeletePipelineTriggerCommand, Core.Commands.ResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public DeletePipelineTriggerCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<Core.Commands.ResultCommand> Handle(DeletePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByIdAsync(request.Id);
			if (pipelineTrigger is null) {
				return new Core.Commands.ResultCommand(HttpStatusCode.NotFound, "The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			_unitOfWork.PipelineTriggerRepository.Remove(pipelineTrigger);
			await _unitOfWork.Commit();

			return new Core.Commands.ResultCommand(HttpStatusCode.NoContent);
		}
	}
}
