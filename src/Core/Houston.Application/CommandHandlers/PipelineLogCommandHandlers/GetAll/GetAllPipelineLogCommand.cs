namespace Houston.Application.CommandHandlers.PipelineLogCommandHandlers.GetAll {
	public sealed record GetAllPipelineLogCommand(Guid PipelineId, int PageSize, int PageIndex) : IRequest<IResultCommand>;
}
