using Houston.Core.Attributes;
using MongoDB.Bson;

namespace Houston.Core.Entities.MongoDB {
	[BsonCollection("pipeline_logs")]
	public class PipelineLogs : EntityBase {
		public ObjectId PipelineId { get; set; }

		public long ExitCode { get; set; }

		public string Stdout { get; set; } = null!;

		public ObjectId? InstructionWithError { get; set; }

		public ObjectId? TriggeredBy { get; set; }

		public DateTime StartTime { get; set; }

		public TimeSpan Duration { get; set; }

		public PipelineLogs() { }

		public PipelineLogs(ObjectId id, ObjectId pipelineId, long exitCode, string stdout, ObjectId? instructionWithError, ObjectId? triggeredBy, DateTime startTime, TimeSpan duration) : base(id) {
			PipelineId = pipelineId;
			ExitCode = exitCode;
			Stdout = stdout ?? throw new ArgumentNullException(nameof(stdout));
			InstructionWithError = instructionWithError;
			TriggeredBy = triggeredBy;
			StartTime = startTime;
			Duration = duration;
		}
	}
}
