using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	public class RepositoryHostTrigger {
		[BsonRequired]
		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonRequired]
		[BsonElement("filters")]
		public List<string> Filters { get; set; } = null!;

		public RepositoryHostTrigger() { }

		public RepositoryHostTrigger(string name, List<string> filters) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Filters = filters ?? throw new ArgumentNullException(nameof(filters));
		}
	}
}
