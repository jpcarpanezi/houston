using Houston.Application.ViewModel;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Interfaces.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ConnectorController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMediator _mediator;

		public ConnectorController(IUnitOfWork unitOfWork, IMediator mediator) {
			_unitOfWork = unitOfWork;
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateConnectorCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			// TODO: Adicionar suporte ao mapper

			return CreatedAtAction(nameof(Create), response.Response);
		}
	}
}
