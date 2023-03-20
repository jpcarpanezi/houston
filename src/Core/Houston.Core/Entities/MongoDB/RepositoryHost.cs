using Houston.Core.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	[BsonCollection("repository_host")]
	public class RepositoryHost : EntityBase {
		[BsonRequired]
		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonRequired]
		[BsonElement("origins")]
		public List<string> Origins { get; set; } = null!;

		[BsonRequired]
		[BsonElement("triggers")]
		public List<RepositoryHostTrigger> Triggers { get; set; } = null!;

		public RepositoryHost() { }

		public RepositoryHost(string name, List<string> origins, List<RepositoryHostTrigger> triggers) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Origins = origins ?? throw new ArgumentNullException(nameof(origins));
			Triggers = triggers ?? throw new ArgumentNullException(nameof(triggers));
		}
	}
}
