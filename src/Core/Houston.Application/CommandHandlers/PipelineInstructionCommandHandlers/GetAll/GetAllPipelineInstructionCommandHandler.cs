namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.GetAll {
	public class GetAllPipelineInstructionCommandHandler : IRequestHandler<GetAllPipelineInstructionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllPipelineInstructionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllPipelineInstructionCommand request, CancellationToken cancellationToken) {
			var pipelineInstructions = await _unitOfWork.PipelineInstructionRepository.GetByPipelineId(request.PipelineId);

			return ResultCommand.Ok<List<PipelineInstruction>, List<PipelineInstructionViewModel>>(pipelineInstructions);
		}
	}
}
