using Houston.API.AuthConfigurations;
using Houston.Application.ViewModel;
using Houston.Core.Commands.AuthCommands;
using Houston.Core.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Projeta.API.Infrastructure;
using System.Net;

namespace Houston.API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly SigningConfigurations _signingConfigurations;
		private readonly TokenConfigurations _tokenConfigurations;
		private readonly IDistributedCache _cache;

		public AuthController(IUnitOfWork unitOfWork, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, IDistributedCache cache) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_signingConfigurations = signingConfigurations ?? throw new ArgumentNullException(nameof(signingConfigurations));
			_tokenConfigurations = tokenConfigurations ?? throw new ArgumentNullException(nameof(tokenConfigurations));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}


		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> SignIn([FromBody] GeneralSignInCommand command) {
			var user = await _unitOfWork.UserRepository.FindByEmail(command.Email);
			
			// TODO: Mudar para validar senha criptografada
			if (user is null || user.Password != command.Password) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("invalidCredentials"));
			}

			if (user.IsActive == false) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("userInactive"));
			}

			return Ok(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}
	}
}
