using Houston.Application.CommandHandlers.AuthCommandHandlers.RefreshToken;
using Houston.Application.CommandHandlers.AuthCommandHandlers.SignIn;

namespace Houston.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class AuthController : ControllerBase {
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator) {
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
		public async Task<IActionResult> SignIn([FromBody] SignInCommand command) => await _mediator.Send(command);

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
		public async Task<IActionResult> RefreshToken(string token) => await _mediator.Send(new RefreshTokenCommand(token));
	}
}
