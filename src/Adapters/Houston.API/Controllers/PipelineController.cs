using Houston.Application.CommandHandlers.PipelineCommandHandlers.Create;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.Delete;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.Get;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.GetAll;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.Run;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.Toggle;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.Update;
using Houston.Application.CommandHandlers.PipelineCommandHandlers.Webhook;
using Houston.Application.ViewModel.PipelineViewModels;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineController : ControllerBase {
		private readonly IMediator _mediator;

		public PipelineController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Creates a new pipeline 
		/// </summary>
		/// <param name="command"></param>
		/// <response code="201">Successfully created pipeline with response object</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(PipelineViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreatePipelineCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Updates a pipeline
		/// </summary>
		/// <param name="command"></param>
		/// <response code="200">Successfully updated the pipeline</response>
		/// <response code="404">The requested pipeline could not be found</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(PipelineViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Update([FromBody] UpdatePipelineCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Deletes a pipeline
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="204">Successfully deleted the pipeline</response>
		/// <response code="404">The requested connector could not be found</response>
		/// <response code="423">Server is processing a request from this pipeline</response>
		[HttpDelete("{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(LockedMessageViewModel), (int)HttpStatusCode.Locked)]
		public async Task<IActionResult> Delete(Guid pipelineId) => await _mediator.Send(new DeletePipelineCommand(pipelineId));

		/// <summary>
		/// Toggle pipeline status to stopped or awaiting
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="204">Successfully toggled pipeline status</response>
		/// <response code="404">The requested connector could not be found</response>
		/// <response code="423">Server is processing a request from this pipeline</response>
		[HttpPatch("toggle/{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(LockedMessageViewModel), (int)HttpStatusCode.Locked)]
		public async Task<IActionResult> ToggleStatus(Guid pipelineId) => await _mediator.Send(new TogglePipelineStatusCommand(pipelineId));

		/// <summary>
		/// Gets the pipeline by id
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="200">Pipeline response</response>
		/// <response code="404">The requested pipeline could not be found</response>
		[HttpGet("{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(PipelineViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid pipelineId) => await _mediator.Send(new GetPipelineCommand(pipelineId));

		/// <summary>
		/// List all active pipelines
		/// </summary>
		/// <param name="command">URL query optional query parameters</param>
		/// <response code="200">List of all active connectors</response>
		[HttpGet]
		[Authorize]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<PipelineViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllPipelineCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Manually runs a pipeline
		/// </summary>
		/// <param name="command"></param>
		/// <response code="204">Pipeline run request accepted</response>
		/// <response code="404">The requested pipeline could not be found</response>
		/// <response code="423">Server is processing a request from this pipeline</response>
		[HttpPost("run")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(LockedMessageViewModel), (int)HttpStatusCode.Locked)]
		public async Task<IActionResult> Run([FromBody] RunPipelineCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Webhook handler to trigger pipeline run
		/// </summary>
		/// <param name="origin">Webhook origin (ex: github)</param>
		/// <param name="pipelineId">Pipeline id to trigger</param>
		/// <response code="204">Pipeline triggered with success</response>
		/// <response code="404">The requested pipeline was not found</response>
		/// <response code="423">The requested pipeline is locked by another proccess</response>
		[HttpPost("webhook/{origin}/{pipelineId:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(LockedMessageViewModel), (int)HttpStatusCode.Locked)]
		public async Task<IActionResult> Webhook(string origin, Guid pipelineId) {
			using var reader = new StreamReader(Request.Body);
			string jsonPayload = await reader.ReadToEndAsync();

			var command = new WebhookCommand(origin, pipelineId, jsonPayload);
			return await _mediator.Send(command);
		}
	}
}
