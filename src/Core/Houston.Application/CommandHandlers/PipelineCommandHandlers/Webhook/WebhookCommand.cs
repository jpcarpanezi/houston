namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Webhook {
	public sealed record WebhookCommand(string Origin, Guid PipelineId, string JsonPayload) : IRequest<IResultCommand>;
}
