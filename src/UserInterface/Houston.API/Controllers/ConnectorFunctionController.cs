﻿using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.ConnectorFunctionViewModels;
using Houston.Core.Commands.ConnectorFunctionCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ConnectorFunctionController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public ConnectorFunctionController(IMediator mediator, IMapper mapper) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Creates a connector function command with inputs
		/// </summary>
		/// <param name="command">Connector function with inputs if necessary</param>
		/// <response code="201">Successfully created connector function</response>
		/// <response code="403">Invalid connector id</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionViewModel), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Create([FromBody] CreateConnectorFunctionCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			var view = _mapper.Map<ConnectorFunctionViewModel>(response.Response);

			return CreatedAtAction(nameof(Create), view);
		}

		/// <summary>
		/// Updates a connector function with inputs
		/// </summary>
		/// <param name="command">Connector function with inputs if necessary</param>
		/// <response code="200">Successfully updated connector function</response>
		/// <response code="403">Invalid connector id</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Update([FromBody] UpdateConnectorFunctionCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			var view = _mapper.Map<ConnectorFunctionViewModel>(response.Response);

			return Ok(view);
		}

		/// <summary>
		/// Logically deletes a connector function with inputs
		/// </summary>
		/// <param name="connectorFunctionId"></param>
		/// <response code="204">Successfully deleted the connector function</response>
		/// <response code="403">Invalid connector function id</response>
		[HttpDelete("{connectorFunctionId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Delete(Guid connectorFunctionId) {
			var command = new DeleteConnectorFunctionCommand(connectorFunctionId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			return NoContent();
		}

		/// <summary>
		/// Gets the connector function with inputs by id
		/// </summary>
		/// <param name="connectorFunctionId"></param>
		/// <response code="200">Connector function response</response>
		/// <response code="404">Connector function not found</response>
		[HttpGet("{connectorFunctionId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorFunctionViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid connectorFunctionId) {
			var command = new GetConnectorFunctionCommand(connectorFunctionId);
			var response = await _mediator.Send(command);

			if (response.StatusCode == HttpStatusCode.NotFound)
				return NotFound();

			var view = _mapper.Map<ConnectorFunctionViewModel>(response.Response);

			return Ok(view);
		}
	}
}
