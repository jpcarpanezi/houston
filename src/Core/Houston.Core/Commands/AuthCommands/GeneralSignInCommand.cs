namespace Houston.Core.Commands.AuthCommands {
	public class GeneralSignInCommand {
		public string Email { get; set; } = null!;

		public string Password { get; set; } = null!;

		public GeneralSignInCommand() { }

		public GeneralSignInCommand(string email, string password) { 
			Email = email;
			Password = password;
		}
	}
}
