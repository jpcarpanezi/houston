using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class UserRepository : Repository<User>, IUserRepository {
		public UserRepository(IMongoContext context) : base(context) { }
	}
}
