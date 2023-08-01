using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.ChangeSecret;
using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create;
using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Delete;
using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Get;
using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.RevealKeys;
using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update;
using Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.UpdateKey;
using Houston.Application.ViewModel.PipelineTriggerViewModels;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineTriggerController : ControllerBase {
		private readonly IMediator _mediator;

		public PipelineTriggerController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Creates a pipeline trigger
		/// </summary>
		/// <param name="command"></param>
		/// <response code="201">Pipeline trigger created successfully</response>
		/// <response code="403">The requested pipeline already has a trigger</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(PipelineTriggerViewModel), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Create([FromBody] CreatePipelineTriggerCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Updates a pipeline trigger
		/// </summary>
		/// <param name="command"></param>
		/// <response code="200">Pipeline trigger updated successfully</response>
		/// <response code="404">The requested pipeline trigger could not be found</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(PipelineTriggerViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Update([FromBody] UpdatePipelineTriggerCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Generates new pipeline trigger deploy keys
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="204">New deploy keys generated successfully</response>
		/// <response code="404">The requested pipeline trigger could not be found</response>
		[HttpPatch("deployKeys/{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> UpdateDeployKeys(Guid pipelineId) => await _mediator.Send(new UpdateDeployKeyCommand(pipelineId));

		/// <summary>
		/// Changes the pipeline trigger secret
		/// </summary>
		/// <param name="command"></param>
		/// <response code="204">Secret updated successfully</response>
		/// <response code="404">The requested pipeline trigger could not be found</response>
		[HttpPatch("changeSecret")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> ChangeSecret([FromBody] ChangeSecretPipelineTriggerCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Deletes a pipeline trigger
		/// </summary>
		/// <param name="pipelineTriggerId"></param>
		/// <response code="204">Pipeline trigger deleted successfully</response>
		/// <response code="404">The requested pipeline trigger could not be found</response>
		[HttpDelete("{pipelineTriggerId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete(Guid pipelineTriggerId) => await _mediator.Send(new DeletePipelineTriggerCommand(pipelineTriggerId));

		/// <summary>
		/// Gets a pipeline trigger by pipeline id
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="200">Pipeline trigger object response</response>
		/// <response code="404">The requested pipeline trigger could not be found</response>
		[HttpGet("{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(PipelineTriggerViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid pipelineId) => await _mediator.Send(new GetPipelineTriggerCommand(pipelineId));

		/// <summary>
		/// Reveals the pipeline trigger deploy keys
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="200">Pipeline trigger deploy keys object response</response>
		/// <response code="404">The requested pipeline trigger could not be found</response>
		/// <response code="403">The deploy keys are already revealed</response>
		[HttpGet("deployKeys/{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(PipelineTriggerKeysViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> RevealKeys(Guid pipelineId) => await _mediator.Send(new RevealPipelineTriggerKeysCommand(pipelineId));
	}
}
