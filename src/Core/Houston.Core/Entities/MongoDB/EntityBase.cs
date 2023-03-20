using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	public abstract class EntityBase {
		[BsonId]
		[BsonRequired]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }
	}
}
