using Houston.Core.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	[BsonCollection("connector_function")]
	public class ConnectorFunction : EntityBase {
		[BsonRequired]
		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonElement("description")]
		public string? Description { get; set; }

		[BsonRequired]
		[BsonElement("connector_id")]
		public ObjectId ConnectorId { get; set; }

		[BsonElement("dependencies")]
		public List<ObjectId>? Dependencies { get; set; }

		[BsonRequired]
		[BsonElement("is_entrypoint")]
		public bool IsEntrypoint { get; set; }

		[BsonRequired]
		[BsonElement("version")]
		[BsonDefaultValue("1.0.0")]
		public string Version { get; set; } = null!;

		[BsonElement("inputs")]
		public List<ConnectorFunctionInput>? Inputs { get; set; }

		[BsonRequired]
		[BsonElement("script")]
		public List<string> Script { get; set; } = null!;

		[BsonRequired]
		[BsonElement("created_by")]
		public ObjectId CreatedBy { get; set; }

		[BsonRequired]
		[BsonElement("creation_date")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime CreationDate { get; set; }

		[BsonRequired]
		[BsonElement("updated_by")]
		public ObjectId UpdatedBy { get; set; }

		[BsonRequired]
		[BsonElement("last_update")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime LastUpdate { get; set; }

		public ConnectorFunction() { }

		public ConnectorFunction(string name, string? description, ObjectId connectorId, List<ObjectId>? dependencies, string version, List<ConnectorFunctionInput>? inputs, List<string> script, ObjectId createdBy, DateTime creationDate, ObjectId updatedBy, DateTime lastUpdate) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
			ConnectorId = connectorId;
			Dependencies = dependencies;
			Version = version ?? "1.0.0";
			Inputs = inputs;
			Script = script ?? throw new ArgumentNullException(nameof(script));
			CreatedBy = createdBy;
			CreationDate = creationDate;
			UpdatedBy = updatedBy;
			LastUpdate = lastUpdate;
		}
	}
}
