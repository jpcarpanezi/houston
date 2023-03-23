using Houston.Core.Entities.MongoDB;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class CreateFirstAccessCommand : IRequest<ResultCommand<User>> {
		public string RegistryAddress { get; set; } = null!;

		public string RegistryEmail { get; set; } = null!;

		public string RegistryUsername { get; set; } = null!;

		public string RegistryPassword { get; set; } = null!;

		public string UserName { get; set; } = null!;

		public string UserEmail { get; set; } = null!;

		public string UserPassword { get; set; } = null!;

		public CreateFirstAccessCommand() { }

		public CreateFirstAccessCommand(string registryAddress, string registryEmail, string registryUsername, string registryPassword, string userName, string userEmail, string userPassword) {
			RegistryAddress = registryAddress ?? throw new ArgumentNullException(nameof(registryAddress));
			RegistryEmail = registryEmail ?? throw new ArgumentNullException(nameof(registryEmail));
			RegistryUsername = registryUsername ?? throw new ArgumentNullException(nameof(registryUsername));
			RegistryPassword = registryPassword ?? throw new ArgumentNullException(nameof(registryPassword));
			UserName = userName ?? throw new ArgumentNullException(nameof(userName));
			UserEmail = userEmail ?? throw new ArgumentNullException(nameof(userEmail));
			UserPassword = userPassword ?? throw new ArgumentNullException(nameof(userPassword));
		}
	}
}
