using AutoMapper;
using Houston.Application.ViewModel;
using Houston.Application.ViewModel.UserViewModels;
using Houston.Core.Commands.UserCommands;
using Houston.Core.Interfaces.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public UserController(IUnitOfWork unitOfWork, IDistributedCache cache, IMediator mediator, IMapper mapper) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Checks if initial configurations were made
		/// </summary>
		/// <response code="200">Initial configurations were made</response>
		/// <response code="404">Initial configurations not found</response>
		[HttpGet("is-first-access")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> IsFirstAccess() {
			var configurations = await _cache.GetStringAsync("configurations");

			if (configurations is null) { 
				return NotFound();
			}

			return Ok();
		}

		/// <summary>
		/// Defines the first user and set Docker Hub credentials
		/// </summary>
		/// <param name="command">First user informations and Docker Hub credentials</param>
		/// <response code="201">User created with success</response>
		/// <responde code="400">Error with user informations</responde>
		/// <response code="403">First access ever made</response>
		[HttpPost("first-access")]
		[ProducesResponseType(typeof(FirstUserViewModel), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> FirstAccess([FromBody] CreateFirstAccessCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			var view = _mapper.Map<FirstUserViewModel>(response.Response);

			return CreatedAtAction(nameof(FirstAccess), view);
		}

		/// <summary>
		/// Creates a new user in the system
		/// </summary>
		/// <param name="command">New user basic informations</param>
		/// <returns>A newly created user</returns>
		/// <response code="201">New user created with success</response>
		/// <response code="400">Error with user informations</response>
		/// <response code="403">User already exists</response>
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(typeof(FirstUserViewModel), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command) {
			var response = await _mediator.Send(command);

			if (response.StatusCode != HttpStatusCode.Created)
				return StatusCode((int)response.StatusCode, new MessageViewModel(response.ErrorMessage!));

			var view = _mapper.Map<FirstUserViewModel>(response.Response);

			return CreatedAtAction(nameof(CreateUser), view);
		}
	}
}
