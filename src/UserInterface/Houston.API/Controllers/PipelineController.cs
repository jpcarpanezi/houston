using AutoMapper;
using EventBus.EventBus.Abstractions;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.PipelineViewModels;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Messages;
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
		private readonly IEventBus _eventBus;
		private readonly ILogger<PipelineController> _logger;

		public PipelineController(IMediator mediator, IMapper mapper, IEventBus eventBus, ILogger<PipelineController> logger) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
		/// <response code="403">Invalid pipeline id</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(PipelineViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Update([FromBody] UpdatePipelineCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			var view = _mapper.Map<PipelineViewModel>(response.Response);

			return Ok(view);
		}

		/// <summary>
		/// Deletes a pipeline
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="204">Successfully deleted the pipeline</response>
		/// <response code="403">Invalid pipeline id</response>
		[HttpDelete("{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Delete(Guid pipelineId) {
			var command = new DeletePipelineCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			return NoContent();
		}

		/// <summary>
		/// Toggle pipeline status to stopped or awaiting
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="204">Successfully toggled pipeline status</response>
		/// <response code="403">Invalid pipeline id</response>
		/// <response code="423">Pipeline is running</response>
		[HttpPatch("toggle/{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(LockedMessageViewModel), (int)HttpStatusCode.Locked)]
		public async Task<IActionResult> ToggleStatus(Guid pipelineId) {
			var command = new TogglePipelineStatusCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.Locked)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			if (response.StatusCode == HttpStatusCode.Locked) { 
				DateTime convertedEstimateTime = DateTime.ParseExact(response.ErrorMessage!, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
				return StatusCode((int)response.StatusCode, new LockedMessageViewModel("pipelineRunning", convertedEstimateTime));
			}

			return NoContent();
		}

		[HttpPost("run")]
		public IActionResult RunPipeline() {
			var message = new RunPipelineMessage("640f51d5681f8ae2d6ae0f15", null);

			try {
				_eventBus.Publish(message);
			} catch (Exception e) {
				_logger.LogError(e, $"Failed to publish {nameof(RunPipelineMessage)}");
			}

			return Ok();
		}
	}
}
