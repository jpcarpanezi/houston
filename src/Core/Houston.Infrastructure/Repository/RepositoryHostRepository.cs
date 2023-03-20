using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class RepositoryHostRepository : Repository<RepositoryHost>, IRepositoryHostRepository {
		public RepositoryHostRepository(IMongoContext context) : base(context) { }
	}
}
