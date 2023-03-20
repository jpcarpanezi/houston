using EventBus.EventBus.Abstractions;
using Houston.Application.ViewModel;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Messages;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IEventBus _eventBus;
		private readonly ILogger<PipelineController> _logger;

		public PipelineController(IMediator mediator, IEventBus eventBus, ILogger<PipelineController> logger) {
			_mediator = mediator;
			_eventBus = eventBus;
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreatePipelineCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			// TODO: Adicionar automapper

			return CreatedAtAction(nameof(Create), response.Response);
		}

		[HttpPatch("instructions")]
		public async Task<IActionResult> UpdateInstructions([FromBody] UpdatePipelineInstructionsCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			return Ok(response.Response);
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
