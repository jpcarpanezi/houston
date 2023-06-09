﻿using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.PipelineTriggerViewModels;
using Houston.Core.Commands.PipelineTriggerCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineTriggerController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public PipelineTriggerController(IMediator mediator, IMapper mapper) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
		public async Task<IActionResult> Create([FromBody] CreatePipelineTriggerCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineTriggerViewModel>(response.Response);

			return CreatedAtAction(nameof(Create), view);
		}

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
		public async Task<IActionResult> Update([FromBody] UpdatePipelineTriggerCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineTriggerViewModel>(response.Response);

			return Ok(view);
		}

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
		public async Task<IActionResult> UpdateDeployKeys(Guid pipelineId) {
			var command = new UpdateDeployKeyCommand(pipelineId);
			var response = await _mediator.Send(command);
			
			if (response.StatusCode != HttpStatusCode.NoContent)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			return NoContent();
		}

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
		public async Task<IActionResult> ChangeSecret([FromBody] ChangeSecretPipelineTriggerCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			return NoContent();
		}

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
		public async Task<IActionResult> Delete(Guid pipelineTriggerId) {
			var command = new DeletePipelineTriggerCommand(pipelineTriggerId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			return NoContent();
		}

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
		public async Task<IActionResult> Get(Guid pipelineId) {
			var command = new GetPipelineTriggerCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineTriggerViewModel>(response.Response);

			return Ok(view);
		}

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
		public async Task<IActionResult> RevealKeys(Guid pipelineId) {
			var command = new RevealPipelineTriggerKeysCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineTriggerKeysViewModel>(response.Response);

			return Ok(view);
		}
	}
}
