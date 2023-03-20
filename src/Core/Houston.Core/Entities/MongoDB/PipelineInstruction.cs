using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	public class PipelineInstruction : EntityBase {
		[BsonRequired]
		[BsonElement("connector_function_id")]
		public ObjectId ConnectorFunctionId { get; set; }

		[BsonElement("comments")]
		public string? Comments { get; set; }

		[BsonRequired]
		[BsonElement("interface_id")]
		public uint InterfaceId { get; set; }

		[BsonElement("connections")]
		public List<uint>? Connections { get; set; } = null!;

		[BsonRequired]
		[BsonElement("pos_x")]
		public uint PosX { get; set; }

		[BsonRequired]
		[BsonElement("pos_y")]
		public uint PosY { get; set; }

		[BsonElement("inputs")]
		public List<PipelineInstructionInput>? Inputs { get; set; }

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

		public PipelineInstruction() { }

		public PipelineInstruction(ObjectId connectorFunctionId, string? comments, uint interfaceId, List<uint>? connections, uint posX, uint posY, List<PipelineInstructionInput>? inputs, List<string> script, ObjectId createdBy, DateTime creationDate, ObjectId updatedBy, DateTime lastUpdate) {
			ConnectorFunctionId = connectorFunctionId;
			Comments = comments;
			InterfaceId = interfaceId;
			Connections = connections;
			PosX = posX;
			PosY = posY;
			Inputs = inputs;
			Script = script ?? throw new ArgumentNullException(nameof(script));
			CreatedBy = createdBy;
			CreationDate = creationDate;
			UpdatedBy = updatedBy;
			LastUpdate = lastUpdate;
		}
	}
}
