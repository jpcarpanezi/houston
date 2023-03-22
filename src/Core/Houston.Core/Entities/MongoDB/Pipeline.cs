using Houston.Core.Attributes;
using Houston.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	[BsonCollection("pipeline")]
	public class Pipeline : EntityBase {
		[BsonRequired]
		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonElement("description")]
		public string? Description { get; set; }

		[BsonRequired]
		[BsonElement("pipeline_status")]
		[BsonDefaultValue(PipelineStatusEnum.Awaiting)]
		public PipelineStatusEnum PipelineStatus { get; set; }

		[BsonRequired]
		[BsonElement("repository_host_config")]
		public ObjectId RepositoryHostConfig { get; set; }

		[BsonRequired]
		[BsonElement("source_code")]
		public string SourceCode { get; set; } = null!;

		[BsonRequired]
		[BsonElement("deploy_key")]
		public string? DeployKey { get; set; }

		[BsonRequired]
		[BsonElement("secret")]
		public string? Secret { get; set; }

		[BsonElement("last_run")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? LastRun { get; set; }

		[BsonRequired]
		[BsonElement("triggers")]
		public List<PipelineTrigger> Triggers { get; set; } = null!;

		[BsonElement("instructions")]
		public List<PipelineInstruction>? Instructions { get; set; } = null!;

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

		public Pipeline() { }

		public Pipeline(ObjectId id, string name, string? description, PipelineStatusEnum pipelineStatus, ObjectId repositoryHostConfig, string sourceCode, string? deployKey, string? secret, DateTime? lastRun, List<PipelineTrigger> triggers, List<PipelineInstruction>? instructions, ObjectId createdBy, DateTime creationDate, ObjectId updatedBy, DateTime lastUpdate) : base(id) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
			PipelineStatus = pipelineStatus;
			RepositoryHostConfig = repositoryHostConfig;
			SourceCode = sourceCode ?? throw new ArgumentNullException(nameof(sourceCode));
			DeployKey = deployKey;
			Secret = secret;
			LastRun = lastRun;
			Triggers = triggers ?? throw new ArgumentNullException(nameof(triggers));
			Instructions = instructions;
			CreatedBy = createdBy;
			CreationDate = creationDate;
			UpdatedBy = updatedBy;
			LastUpdate = lastUpdate;
		}
	}
}
