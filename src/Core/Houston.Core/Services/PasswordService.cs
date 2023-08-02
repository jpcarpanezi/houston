namespace Houston.Core.Services {
	public static class PasswordService {
		private static readonly Regex containsUpperCase = new("[A-Z]");
		private static readonly Regex containsNumber = new(@"\d");

		public static bool IsPasswordStrong(string password) {
			if (password.Length < 8) {
				return false;
			}

			if (!containsUpperCase.IsMatch(password)) {
				return false;
			}

			if (!containsNumber.IsMatch(password)) {
				return false;
			}

			return true;
		}

		public static string HashPassword(string password) {
			return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA384);
		}

		public static bool VerifyHashedPassword(string providedPassword, string hashedPassword) {
			return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword, HashType.SHA384);
		}
	}
}
