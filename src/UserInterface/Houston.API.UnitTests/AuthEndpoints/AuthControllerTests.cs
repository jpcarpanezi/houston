using Houston.Core.Entities.Postgres;
using Houston.Core.Services;

namespace Houston.API.UnitTests.AuthEndpoints {
	[TestFixture]
	public class AuthControllerTests {
		private AuthController _authController;
		private Mock<IUnitOfWork> _mockUnitOfWork;
		private IDistributedCache _cache;
		private TokenConfigurations _tokenConfigurations;

		[SetUp]
		public void SetUp() {
			var signingConfigurations = new SigningConfigurations(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PrivateKey.pem")));
			_tokenConfigurations = new TokenConfigurations("houston", "houston", 1200, 3600);
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
			_authController = new AuthController(_mockUnitOfWork.Object, signingConfigurations, _tokenConfigurations, _cache);
		}

		[Test]
		public async Task SignIn_WithValidCredentials_ReturnsOk() {
			// Arrange
			var command = new GeneralSignInCommand("test@test.com", "password");
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(u => u.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await _authController.SignIn(command) as OkObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.TypeOf<BearerTokenViewModel>());
			});
		}

		[Test]
		public async Task SignIn_WithValidCredentialsFirstAccess_ReturnsUnauthorized() {
			// Arrange
			var command = new GeneralSignInCommand("test@test.com", "password");
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = true,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(u => u.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await _authController.SignIn(command) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(307));
				Assert.That(result.Value, Is.TypeOf<FirstAccessViewModel>());
			});
		}

		[Test]
		public async Task SignIn_WithInvalidCredentials_ReturnsForbidden() {
			// Arrange
			var command = new GeneralSignInCommand("test@test.com", "wrongpassword");
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(u => u.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await _authController.SignIn(command) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(403));
				Assert.That(result.Value, Is.TypeOf<MessageViewModel>());
				Assert.That(((MessageViewModel)result.Value).Message, Is.Not.Null);
				Assert.That(((MessageViewModel)result.Value).ErrorCode, Is.EqualTo("invalidCredentials"));
			});
		}

		[Test]
		public async Task SignIn_WithInactiveUser_ReturnsForbidden() {
			// Arrange
			var command = new GeneralSignInCommand("test@test.com", "password");
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = false,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(u => u.UserRepository.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);

			// Act
			var result = await _authController.SignIn(command) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(401));
				Assert.That(result.Value, Is.TypeOf<MessageViewModel>());
				Assert.That(((MessageViewModel)result.Value).Message, Is.Not.Null);
				Assert.That(((MessageViewModel)result.Value).ErrorCode, Is.EqualTo("userInactive"));
			});
		}

		[Test]
		public async Task RefreshToken_WithValidToken_ReturnsOk() {
			// Arrange
			Guid userId = Guid.NewGuid();
			var token = Guid.NewGuid().ToString("N");
			await InsertTokenData(token, userId);
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = true,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

			// Act
			var result = await _authController.RefreshToken(token) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.Value, Is.TypeOf<BearerTokenViewModel>());
				Assert.That(result.StatusCode, Is.EqualTo(200));
			});
		}

		[Test]
		public async Task RefreshToken_WithInvalidToken_ReturnsForbidden() {
			// Arrange
			string token = "";

			// Act
			var result = await _authController.RefreshToken(token) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(403));
				Assert.That(result.Value, Is.TypeOf<MessageViewModel>());
				Assert.That(((MessageViewModel)result.Value).Message, Is.Not.Null);
				Assert.That(((MessageViewModel)result.Value).ErrorCode, Is.EqualTo("invalidToken"));
			});
		}

		[Test]
		public async Task RefreshToken_WithValidTokenAndUserNotFound_ReturnsForbidden() {
			// Arrange
			Guid userId = Guid.NewGuid();
			string token = Guid.NewGuid().ToString("N");
			await InsertTokenData(token, userId);
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(default(User));

			// Act
			var result = await _authController.RefreshToken(token) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(403));
				Assert.That(result.Value, Is.TypeOf<MessageViewModel>());
				Assert.That(((MessageViewModel)result.Value).Message, Is.Not.Null);
				Assert.That(((MessageViewModel)result.Value).ErrorCode, Is.EqualTo("userNotFound"));
			});
		}

		[Test]
		public async Task RefreshToken_WithValidTokenAndInactiveUser_ReturnsForbidden() {
			// Arrange
			Guid userId = Guid.NewGuid();
			string token = Guid.NewGuid().ToString("N");
			await InsertTokenData(token, userId);
			var user = new User {
				Id = Guid.NewGuid(),
				Name = "John Doe",
				Email = "john.doe@example.com",
				Password = PasswordService.HashPassword("password"),
				Active = false,
				Role = Core.Enums.UserRoleEnum.Admin,
				FirstAccess = false,
				CreatedBy = It.IsAny<Guid>(),
				CreationDate = It.IsAny<DateTime>(),
				UpdatedBy = It.IsAny<Guid>(),
				LastUpdate = It.IsAny<DateTime>()
			};
			_mockUnitOfWork.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user);

			// Act
			var result = await _authController.RefreshToken(token) as ObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(result.StatusCode, Is.EqualTo(403));
				Assert.That(result.Value, Is.TypeOf<MessageViewModel>());
				Assert.That(((MessageViewModel)result.Value).Message, Is.Not.Null);
				Assert.That(((MessageViewModel)result.Value).ErrorCode, Is.EqualTo("userNotFound"));
			});
		}

		private async Task InsertTokenData(string token, Guid userId, string userEmail = "test@test.com") {
			var tokenData = new RefreshTokenData(token, userId.ToString(), userEmail);

			var opts = new DistributedCacheEntryOptions {
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_tokenConfigurations.FinalExpiration)
			};

			await _cache.SetStringAsync(token, JsonSerializer.Serialize(tokenData), opts);
		}
	}
}
