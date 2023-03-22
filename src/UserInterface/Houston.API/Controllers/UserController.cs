using Houston.Core.Interfaces.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;
		private readonly IMediator _mediator;

		public UserController(IUnitOfWork unitOfWork, IDistributedCache cache, IMediator mediator) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpGet("is-first-access")]
		public async Task<IActionResult> IsFirstAccess() {
			var configurations = await _cache.GetStringAsync("configurations");

			if (configurations is null) { 
				return NotFound();
			}

			return Ok();
		}
	}
}
