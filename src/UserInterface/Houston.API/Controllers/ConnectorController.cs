﻿using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.ConnectorViewModels;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Interfaces.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ConnectorController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public ConnectorController(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mediator = mediator;
			_mapper = mapper;
		}

		/// <summary>
		/// Creates a new connector
		/// </summary>
		/// <param name="command">Connector parameters</param>
		/// <response code="201">Created connector</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(ConnectorViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Create([FromBody] CreateConnectorCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			var view = _mapper.Map<ConnectorViewModel>(response.Response);

			return CreatedAtAction(nameof(Create), view);
		}
	}
}
