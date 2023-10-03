using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Create;

namespace Houston.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class ConnectorFunctionHistoryController : ControllerBase {
		private readonly IMediator _mediator;

		public ConnectorFunctionHistoryController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateConnectorFunctionHistoryCommand command) => await _mediator.Send(command);
	}
}
