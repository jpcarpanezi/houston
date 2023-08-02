namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Delete {
	public class DeletePipelineTriggerCommandHandler : IRequestHandler<DeletePipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public DeletePipelineTriggerCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(DeletePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByIdAsync(request.Id);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			_unitOfWork.PipelineTriggerRepository.Remove(pipelineTrigger);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
