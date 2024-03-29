﻿namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public sealed record CreateConnectorFunctionCommand(string Name, string? Description, Guid ConnectorId) : IRequest<IResultCommand>;
}
