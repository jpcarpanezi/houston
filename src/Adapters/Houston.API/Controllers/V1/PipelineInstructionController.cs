using Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.GetAll;
using Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.Save;
using Houston.Application.ViewModel.PipelineInstructionViewModels;

namespace Houston.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class PipelineInstructionController : ControllerBase {
		private readonly IMediator _mediator;

		public PipelineInstructionController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Saves the pipeline instructions
		/// </summary>
		/// <param name="command"></param>
		/// <response code="201">Pipeline instructions successfully saved</response>
		/// <response code="403">Invalid instruction inputs</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(List<PipelineInstructionViewModel>), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Save([FromBody] SavePipelineInstructionCommand command) => await _mediator.Send(command);

		/// <summary>
		/// Gets instruction by pipeline id
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="200">List of all instructions from provided pipeline</response>
		[HttpGet("{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(List<PipelineInstructionViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll(Guid pipelineId) => await _mediator.Send(new GetAllPipelineInstructionCommand(pipelineId));
	}
}
