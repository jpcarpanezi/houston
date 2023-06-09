﻿using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.ConnectorViewModels;
using Houston.Core.Commands.ConnectorCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ConnectorController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public ConnectorController(IMediator mediator, IMapper mapper) {
			_mediator = mediator;
			_mapper = mapper;
		}

		/// <summary>
		/// Creates a new connector
		/// </summary>
		/// <param name="command">Connector parameters</param>
		/// <response code="201">Connector created successfully</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreateConnectorCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<ConnectorViewModel>(response.Response);

			return CreatedAtAction(nameof(Create), view);
		}

		/// <summary>
		/// Logically deletes a connector
		/// </summary>
		/// <param name="connectorId"></param>
		/// <response code="204">Connector deleted successfully</response>
		/// <response code="404">The requested connector could not be found</response>
		[HttpDelete("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete(Guid connectorId) {
			var command = new DeleteConnectorCommand(connectorId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.NoContent)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			return NoContent();
		}

		/// <summary>
		/// Updates connector informations
		/// </summary>
		/// <param name="command">Connector informations</param>
		/// <response code="200">Connector successfully updated</response>
		/// <response code="404">The requested connector could not be found</response>
		[HttpPut]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Update([FromBody] UpdateConnectorCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<ConnectorViewModel>(response.Response);

			return Ok(view);
		}

		/// <summary>
		/// Gets the connector by id
		/// </summary>
		/// <param name="connectorId"></param>
		/// <response code="200">Connector response</response>
		/// <response code="404">The requested connector could not be found</response>
		[HttpGet("{connectorId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid connectorId) {
			var command = new GetConnectorCommand(connectorId);
			var response = await _mediator.Send(command);

			if (response.StatusCode == HttpStatusCode.NotFound)
				return NotFound(new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<ConnectorViewModel>(response.Response);

			return Ok(view);
		}

		/// <summary>
		/// List all active connectors
		/// </summary>
		/// <param name="command">URL query optional query parameters</param>
		/// <response code="200">List of all active connectors</response>
		[HttpGet]
		[Authorize]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<ConnectorViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllConnectorCommand command) {
			var response = await _mediator.Send(command);

			var view = _mapper.Map<List<ConnectorViewModel>>(response.Response);

			return Ok(new PaginatedItemsViewModel<ConnectorViewModel>(response.PageIndex, response.PageSize, response.Count, view));
		}
	}
}
