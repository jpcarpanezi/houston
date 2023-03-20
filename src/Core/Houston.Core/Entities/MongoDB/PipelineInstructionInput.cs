using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	public class PipelineInstructionInput {
		[BsonRequired]
		[BsonElement("replace")]
		public string Replace { get; set; } = null!;

		[BsonRequired]
		[BsonElement("value")]
		public string Value { get; set; } = null!;

		public PipelineInstructionInput() { }

		public PipelineInstructionInput(string replace, string value) {
			Replace = replace ?? throw new ArgumentNullException(nameof(replace));
			Value = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}
