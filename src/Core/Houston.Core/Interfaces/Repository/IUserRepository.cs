using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User> {
		Task<User?> FindByEmail(string email);

		Task<bool> AnyUser();
	}
}
