using Houston.Application.ViewModel;
using Houston.Core.Commands.ConnectorFunctionCommands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ConnectorFunctionController : ControllerBase {
		private readonly IMediator _mediator;

		public ConnectorFunctionController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateConnectorFunctionCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			// TODO: Adicionar suporte ao AutoMapper

			return CreatedAtAction(nameof(Create), response.Response);
		}
	}
}
