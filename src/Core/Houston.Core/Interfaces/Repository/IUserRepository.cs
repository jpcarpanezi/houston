using Houston.Core.Entities.MongoDB;

namespace Houston.Core.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User> {
		Task<User?> FindByEmail(string email);
	}
}
