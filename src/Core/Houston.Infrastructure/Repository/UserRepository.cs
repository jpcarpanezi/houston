using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Houston.Infrastructure.Repository {
	public class UserRepository : Repository<User>, IUserRepository {
		public UserRepository(IMongoContext context) : base(context) { }

		public async Task<User?> FindByEmail(string email) {
			return await DbSet.AsQueryable().Where(x => x.Email == email).FirstOrDefaultAsync();
		}
	}
}
