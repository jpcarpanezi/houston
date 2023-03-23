using Houston.API.AuthConfigurations;
using Houston.Application.ViewModel;
using Houston.Core.Commands.AuthCommands;
using Houston.Core.Entities.Redis;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
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
		/// <returns>A JWT with refresh token to send in header for authorization from API endpoints</returns>
		/// <response code="200">Return a JWT</response>
		/// <response code="403">If user is inactive or with invalid credentials</response>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(BearerTokenViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> SignIn([FromBody] GeneralSignInCommand command) {
			var user = await _unitOfWork.UserRepository.FindByEmail(command.Email);

			// TODO: Mudar para validar senha criptografada
			if (user is null || !PasswordService.VerifyHashedPassword(command.Password, user.Password)) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("invalidCredentials"));
			}

			if (user.IsActive == false) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("userInactive"));
			}

			return Ok(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}

		/// <summary>
		/// Refreshes user JWT token
		/// </summary>
		/// <param name="token">Refresh token given with JWT on authentication</param>
		/// <returns>A new JWT and refresh token</returns>
		/// <response code="200">Return a new JWT with refresh token</response>
		/// <response code="403">If token or user are invalid</response>
		[HttpGet("{token}")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(BearerTokenViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.Forbidden)]
		public async Task<IActionResult> RefreshToken(string token) {
			if (string.IsNullOrWhiteSpace(token)) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("invalidToken"));
			}

			string? redisToken = await _cache.GetStringAsync(token);
			if (redisToken is null) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("tokenExpired"));
			}

			RefreshTokenData? tokenData = JsonSerializer.Deserialize<RefreshTokenData>(redisToken);
			if (tokenData is null) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("tokenExpired"));
			}

			var user = await _unitOfWork.UserRepository.FindByIdAsync(ObjectId.Parse(tokenData.UserId));
			if (user is null) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("userNotFound"));
			}

			if (!user.IsActive) {
				return StatusCode((int)HttpStatusCode.Forbidden, new MessageViewModel("userInactive"));
			}

			await _cache.RemoveAsync(token);

			return Ok(await TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations, _cache));
		}
	}
}
