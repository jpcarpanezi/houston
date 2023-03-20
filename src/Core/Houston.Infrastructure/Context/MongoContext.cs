using Houston.Core.Attributes;
using Houston.Core.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Houston.Infrastructure.Context {
	public class MongoContext : IMongoContext {
		private MongoClient _mongoClient { get; set; }

		private IMongoDatabase _mongoDatabase { get; set; }
		

		public MongoContext(IConfiguration configuration) {
			_mongoClient = new MongoClient(configuration["ConnectionStrings:MongoDB"]);
			_mongoDatabase = _mongoClient.GetDatabase(configuration["ConnectionStrings:MongoDatabase"]);
		}

		public IMongoCollection<T> GetCollection<T>(Type documentType) {
			BsonCollectionAttribute? attribute = (BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).Single();

			if (attribute is null)
				throw new ArgumentNullException(nameof(attribute));

			return _mongoDatabase.GetCollection<T>(attribute.CollectionName);
		}
	}
}
