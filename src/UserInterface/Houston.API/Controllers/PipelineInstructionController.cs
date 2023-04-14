using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.PipelineInstructionViewModels;
using Houston.Core.Commands.PipelineInstructionCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineInstructionController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public PipelineInstructionController(IMediator mediator, IMapper mapper) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Saves the pipeline instructions
		/// </summary>
		/// <param name="command"></param>
		/// <response code="201">Pipeline instructions successfully saved</response>
		/// <response code="403">Invalid instruction inputs</response>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(List<PipelineInstructionViewModel>), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> Save([FromBody] SavePipelineInstructionCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<List<PipelineInstructionViewModel>>(response.Response);

			return CreatedAtAction(nameof(Save), view);
		}

		/// <summary>
		/// Gets instruction by pipeline id
		/// </summary>
		/// <param name="pipelineId"></param>
		/// <response code="200">List of all instructions from provided pipeline</response>
		[HttpGet("{pipelineId:guid}")]
		[Authorize]
		[ProducesResponseType(typeof(List<PipelineInstructionViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAll(Guid pipelineId) {
			var command = new GetAllPipelineInstructionCommand(pipelineId);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<List<PipelineInstructionViewModel>>(response.Response);

			return Ok(view);
		}
	}
}
