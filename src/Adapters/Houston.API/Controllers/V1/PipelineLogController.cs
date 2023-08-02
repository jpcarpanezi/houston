using Houston.Application.CommandHandlers.PipelineLogCommandHandlers.Get;
using Houston.Application.CommandHandlers.PipelineLogCommandHandlers.GetAll;

namespace Houston.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class PipelineLogController : ControllerBase {
		private readonly IMediator _mediator;

		public PipelineLogController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Gets a pipeline run log by id
		/// </summary>
		/// <param name="id"></param>
		/// <response code="200">Pipeline log object response</response>
		/// <response code="404">The requested pipeline log could not be found</response>
		[HttpGet("item/{id:guid}")]
		public async Task<IActionResult> Get(Guid id) => await _mediator.Send(new GetPipelineLogCommand(id));

		/// <summary>
		/// Gets pipeline runs history log
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <response code="200">Pipeline logs list object response</response>
		[HttpGet("{pipelineId:guid}")]
		public async Task<IActionResult> GetAll(Guid pipelineId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0) => await _mediator.Send(new GetAllPipelineLogCommand(pipelineId, pageSize, pageIndex));
	}
}
