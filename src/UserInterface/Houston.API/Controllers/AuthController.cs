using Houston.API.AuthConfigurations;
using Houston.Application.ViewModel;
using Houston.Core.Commands.AuthCommands;
using Houston.Core.Entities.Redis;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Projeta.API.Infrastructure;
using System.Net;
using System.Text.Json;

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

		/// <summary>
		/// Performs user authentication
		/// </summary>
		/// <param name="command">JSON containing authentication fields</param>
		/// <response code="200">Successfully logged in</response>
		/// <response code="307">First login. Need to create a new password to continue</response>
		/// <response code="401">The account has been deactivated</response>
		/// <response code="403">Invalid username or password</response>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(BearerTokenViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FirstAccessViewModel), (int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(FirstAccessViewModel), (int)HttpStatusCode.TemporaryRedirect)]
		public async Task<IActionResult> SignIn([FromBody] GeneralSignInCommand command) {
			var user = await _unitOfWork.UserRepository.FindByEmail(command.Email);

			if (user is null || !PasswordService.VerifyHashedPassword(command.Password, user.Password)) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("Invalid username or password.", "invalidCredentials"));
			}

			if (!user.Active) {
				return Unauthorized(new MessageViewModel("The account has been deactivated.", "userInactive"));
			}

			if (user.FirstAccess) {
				var passwordToken = await _cache.GetStringAsync(user.Id.ToString());

				if (passwordToken is null) {
					passwordToken = Guid.NewGuid().ToString("N");
					DistributedCacheEntryOptions cacheOptions = new();
					cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
					await _cache.SetStringAsync(user.Id.ToString(), passwordToken, cacheOptions);
				}

				return StatusCode((int)HttpStatusCode.TemporaryRedirect, new FirstAccessViewModel(passwordToken, $"/api/User/firstAccess/{passwordToken}", "First login. Need to create a new password to continue.", "firstAccess"));
			}

			return Ok(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}

		/// <summary>
		/// Refreshes user JWT token
		/// </summary>
		/// <param name="token">Refresh token given with JWT on authentication</param>
		/// <response code="200">JWT successfully refreshed</response>
		/// <response code="403">Invalid or expired token</response>
		[HttpGet("{token}")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(BearerTokenViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> RefreshToken(string token) {
			if (string.IsNullOrWhiteSpace(token)) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("Invalid token.", "invalidToken"));
			}

			string? redisToken = await _cache.GetStringAsync(token);
			if (redisToken is null) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("Token expired.", "invalidToken"));
			}

			RefreshTokenData? tokenData = JsonSerializer.Deserialize<RefreshTokenData>(redisToken);
			var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(tokenData!.UserId));
			if (user is null) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("User not found.", "userNotFound"));
			}

			if (!user.Active) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("User inactive.", "userNotFound"));
			}

			await _cache.RemoveAsync(token);

			return Ok(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}
	}
}
