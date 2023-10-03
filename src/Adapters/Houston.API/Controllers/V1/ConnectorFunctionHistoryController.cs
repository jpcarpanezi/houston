using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Create;
using Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Delete;
using Houston.Application.ViewModel.ConnectorFunctionHistoryViewModels;

namespace Houston.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class ConnectorFunctionHistoryController : ControllerBase {
		private readonly IMediator _mediator;

		public ConnectorFunctionHistoryController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Creates a new connector function version.
		/// </summary>
		/// <param name="command">The connector function data for the new version.</param>
		/// <response code="201">Returns the newly created connector function version.</response>
		[HttpPost]
		[ProducesResponseType(typeof(ConnectorFunctionHistoryDetailViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreateConnectorFunctionHistoryCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Deletes a connector function version.
		/// </summary>
		/// <param name="id">The unique identifier of the connector function version to be deleted.</param>
		/// <response code="204">The connector function version was successfully deleted.</response>
		/// <response code="404">The connector function version could not be found.</response>
		[HttpDelete("{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete(Guid id) => await _mediator.Send(new DeleteConnectorFunctionHistoryCommand(id));
	}
}
