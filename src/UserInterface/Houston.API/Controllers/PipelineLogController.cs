using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.PipelineLogViewModels;
using Houston.Core.Commands.PipelineLogCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PipelineLogController : ControllerBase {
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public PipelineLogController(IMediator mediator, IMapper mapper) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Gets a pipeline run log by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> Get(Guid id) {
			var command = new GetPipelineLogCommand(id);
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!, response.ErrorCode));

			var view = _mapper.Map<PipelineLogViewModel>(response.Response);

			return Ok(view);
		}
	}
}
