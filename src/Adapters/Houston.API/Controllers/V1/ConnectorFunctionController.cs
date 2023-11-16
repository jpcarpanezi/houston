using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Delete;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Get;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.GetAll;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update;
using Houston.Application.ViewModel.ConnectorFunctionViewModels;

namespace Houston.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class ConnectorFunctionController : ControllerBase {
		private readonly IMediator _mediator;

		public ConnectorFunctionController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Creates a connector function command with inputs
		/// </summary>
		/// <param name="command">Connector function with inputs if necessary</param>
		/// <response code="201">Successfully created connector function</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionDetailViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromForm] CreateConnectorFunctionCommand command) => await _mediator.Send(command);
		
		/// <summary>
		/// Updates a connector function with inputs
		/// </summary>
		/// <param name="connectorId">The unique identifier of the connector function to be updated.</param>
		/// <param name="command">The command containing updated information for the connector function.</param>
		/// <response code="200">The connector function was successfully updated.</response>
		/// <response code="404">The requested connector function could not be found.</response>
		[HttpPut("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionDetailViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Update(Guid connectorId, [FromForm] UpdateConnectorFunctionFilesCommand command) => await _mediator.Send(new UpdateConnectorFunctionCommand(connectorId, command.SpecFile, command.Script, command.Package));

		/// <summary>
		/// Logically deletes a connector function with inputs
		/// </summary>
		/// <param name="connectorFunctionId"></param>
		/// <response code="204">Successfully deleted the connector function</response>
		/// <response code="404">The requested connector function could not be found</response>
		[HttpDelete("{connectorFunctionId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete(Guid connectorFunctionId) => await _mediator.Send(new DeleteConnectorFunctionCommand(connectorFunctionId));

		/// <summary>
		/// Gets the connector function with inputs by id
		/// </summary>
		/// <param name="connectorFunctionId"></param>
		/// <response code="200">Connector function response</response>
		/// <response code="404">The requested connector function could not be found</response>
		[HttpGet("item/{connectorFunctionId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionDetailViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid connectorFunctionId) => await _mediator.Send(new GetConnectorFunctionCommand(connectorFunctionId));

		/// <summary>
		/// Retrieve a paginated list of active connector functions for a specific connector.
		/// </summary>
		/// <param name="connectorId">The unique identifier of the connector.</param>
		/// <param name="pageSize">The number of items to include per page.</param>
		/// <param name="pageIndex">The page index (starting from 0).</param>
		/// <response code="200">Returns a paginated list of active connector functions.</response>
		[HttpGet("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<ConnectorFunctionGroupedViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll(Guid connectorId, [FromQuery] int pageSize, [FromQuery] int pageIndex) => await _mediator.Send(new GetAllConnectorFunctionCommand(connectorId, pageSize, pageIndex));
	}
}
