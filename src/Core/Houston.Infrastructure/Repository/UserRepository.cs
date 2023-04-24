using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class UserRepository : Repository<User>, IUserRepository {
		public UserRepository(PostgresContext context) : base(context) { }

		public async Task<bool> AnyUser() {
			return await Context.User.AnyAsync();
		}

		public async Task<long> Count() {
			return await Context.User.LongCountAsync();
		}

		public async Task<User?> FindByEmail(string email) {
			return await Context.User.Where(x => x.Email == email).FirstOrDefaultAsync();
		}

		public async Task<List<User>> GetAll(int pageSize, int pageIndex) {
			return await Context.User.OrderBy(x => x.Name)
								 .Where(x => x.Active)
								 .Skip(pageSize * pageIndex)
								 .Take(pageSize)
								 .ToListAsync();
		}
	}
}
