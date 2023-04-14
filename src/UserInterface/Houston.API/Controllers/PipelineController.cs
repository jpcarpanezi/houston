using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.PipelineViewModels;
using Houston.Core.Commands.PipelineCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public PipelineController(IMediator mediator, IMapper mapper) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Creates a new pipeline 
		/// </summary>
		/// <param name="command"></param>
		/// <response code="201">Successfully created pipeline with response object</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(PipelineViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreatePipelineCommand command) {
			var response = await _mediator.Send(command);

			var view = _mapper.Map<PipelineViewModel>(response.Response);

			return CreatedAtAction(nameof(Create), view);
		}

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
		public async Task<IActionResult> Update([FromBody] UpdatePipelineCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineViewModel>(response.Response);

			return Ok(view);
		}

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
		public async Task<IActionResult> Delete(Guid pipelineId) {
			var command = new DeletePipelineCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.Locked)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			if (response.StatusCode == HttpStatusCode.Locked) {
				DateTime convertedEstimateTime = DateTime.ParseExact(response.ErrorMessage!, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
				return StatusCode((int)response.StatusCode, new LockedMessageViewModel("Server is processing a request from this pipeline. Please try again later.", "pipelineRunning", convertedEstimateTime));
			}

			return NoContent();
		}

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
		public async Task<IActionResult> ToggleStatus(Guid pipelineId) {
			var command = new TogglePipelineStatusCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.Locked)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			if (response.StatusCode == HttpStatusCode.Locked) { 
				DateTime convertedEstimateTime = DateTime.ParseExact(response.ErrorMessage!, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
				return StatusCode((int)response.StatusCode, new LockedMessageViewModel("Server is processing a request from this pipeline. Please try again later.", "pipelineRunning", convertedEstimateTime));
			}

			return NoContent();
		}

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
		public async Task<IActionResult> Get(Guid pipelineId) {
			var command = new GetPipelineCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineViewModel>(response.Response);

			return Ok(view);
		}

		/// <summary>
		/// List all active pipelines
		/// </summary>
		/// <param name="command">URL query optional query parameters</param>
		/// <response code="200">List of all active connectors</response>
		[HttpGet]
		[Authorize]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<PipelineViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllPipelineCommand command) {
			var response = await _mediator.Send(command);

			var view = _mapper.Map<List<PipelineViewModel>>(response.Response);

			return Ok(new PaginatedItemsViewModel<PipelineViewModel>(response.PageIndex, response.PageSize, response.Count, view));
		}

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
		public async Task<IActionResult> Run([FromBody] RunPipelineCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.Locked)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			if (response.StatusCode == HttpStatusCode.Locked) {
				DateTime convertedEstimateTime = DateTime.ParseExact(response.ErrorMessage!, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
				return StatusCode((int)response.StatusCode, new LockedMessageViewModel("Server is processing a request from this pipeline. Please try again later.", "pipelineRunning", convertedEstimateTime));
			}

			return NoContent();
		}
	}
}
