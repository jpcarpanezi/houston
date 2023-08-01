namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.GetAll {
	public sealed record GetAllPipelineCommand(int PageSize, int PageIndex) : IRequest<IResultCommand>;
}
