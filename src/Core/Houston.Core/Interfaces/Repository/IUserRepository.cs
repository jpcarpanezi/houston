namespace Houston.Core.Interfaces.Repository {
	public interface IUserRepository : IRepository<User> {
		Task<User?> FindByEmail(string email);

		Task<bool> AnyUser();

		Task<long> Count();

		Task<List<User>> GetAll(int pageSize, int pageIndex);
	}
}
