using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Delete;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Get;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.GetAll;
using Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update;
using Houston.Application.ViewModel.ConnectorFunctionViewModels;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
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
		[ProducesResponseType(typeof(ConnectorFunctionViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreateConnectorFunctionCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Updates a connector function with inputs
		/// </summary>
		/// <param name="command">Connector function with inputs if necessary</param>
		/// <response code="200">Successfully updated connector function</response>
		/// <response code="404">The requested connector function could not be found</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Update([FromBody] UpdateConnectorFunctionCommand command) => await _mediator.Send(command);

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
		[ProducesResponseType(typeof(ConnectorFunctionViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid connectorFunctionId) => await _mediator.Send(new GetConnectorFunctionCommand(connectorFunctionId));

		/// <summary>
		/// List all active connector function with inputs
		/// </summary>
		/// <param name="command">URL query optional query parameters</param>
		/// <response code="200">List of all active connector functions</response>
		[HttpGet("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<ConnectorFunctionViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllConnectorFunctionCommand command, Guid connectorId) => await _mediator.Send(command);
	}
}
