using MongoDB.Driver;

namespace Houston.Core.Interfaces.Repository {
	public interface IMongoContext {
		IMongoCollection<T> GetCollection<T>(Type documentType);
	}
}
