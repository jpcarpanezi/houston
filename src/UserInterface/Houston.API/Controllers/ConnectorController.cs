using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Create;
using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Delete;
using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Get;
using Houston.Application.CommandHandlers.ConnectorCommandHandlers.GetAll;
using Houston.Application.CommandHandlers.ConnectorCommandHandlers.Update;
using Houston.Application.ViewModel.ConnectorViewModels;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ConnectorController : ControllerBase {
		private readonly IMediator _mediator;

		public ConnectorController(IMediator mediator) {
			_mediator = mediator;
		}

		/// <summary>
		/// Creates a new connector
		/// </summary>
		/// <param name="command">Connector parameters</param>
		/// <response code="201">Connector created successfully</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreateConnectorCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Logically deletes a connector
		/// </summary>
		/// <param name="connectorId"></param>
		/// <response code="204">Connector deleted successfully</response>
		/// <response code="404">The requested connector could not be found</response>
		[HttpDelete("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete(Guid connectorId) => await _mediator.Send(new DeleteConnectorCommand(connectorId));

		/// <summary>
		/// Updates connector informations
		/// </summary>
		/// <param name="command">Connector informations</param>
		/// <response code="200">Connector successfully updated</response>
		/// <response code="404">The requested connector could not be found</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Update([FromBody] UpdateConnectorCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Gets the connector by id
		/// </summary>
		/// <param name="connectorId"></param>
		/// <response code="200">Connector response</response>
		/// <response code="404">The requested connector could not be found</response>
		[HttpGet("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid connectorId) => await _mediator.Send(new GetConnectorCommand(connectorId));

		/// <summary>
		/// List all active connectors
		/// </summary>
		/// <param name="command">URL query optional query parameters</param>
		/// <response code="200">List of all active connectors</response>
		[HttpGet]
		[Authorize]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<ConnectorViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllConnectorCommand command) => await _mediator.Send(command);
	}
}
