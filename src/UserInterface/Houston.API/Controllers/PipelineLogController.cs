using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.ConnectorFunctionViewModels;
using Houston.Application.ViewModel.PipelineLogViewModels;
using Houston.Core.Commands.PipelineLogCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineLogController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public PipelineLogController(IMediator mediator, IMapper mapper) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Gets a pipeline run log by id
		/// </summary>
		/// <param name="id"></param>
		/// <response code="200">Pipeline log object response</response>
		/// <response code="404">The requested pipeline log could not be found</response>
		[HttpGet("item/{id:guid}")]
		public async Task<IActionResult> Get(Guid id) {
			var command = new GetPipelineLogCommand(id);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineLogViewModel>(response.Response);

			return Ok(view);
		}

		/// <summary>
		/// Gets pipeline runs history log
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <response code="200">Pipeline logs list object response</response>
		[HttpGet("{pipelineId:guid}")]
		public async Task<IActionResult> GetAll(Guid pipelineId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0) {
			var command = new GetAllPipelineLogCommand(pipelineId, pageSize, pageIndex);
			var response = await _mediator.Send(command);

			var view = _mapper.Map<List<PipelineLogViewModel>>(response.Response);

			return Ok(new PaginatedItemsViewModel<PipelineLogViewModel>(response.PageIndex, response.PageSize, response.Count, view));
		}
	}
}
