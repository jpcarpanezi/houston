using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("PipelineLog", Schema = "houston")]
public partial class PipelineLog {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_id")]
	public Guid PipelineId { get; set; }

	[Column("exit_code")]
	public int ExitCode { get; set; }

	[Column("stdout")]
	public string? Stdout { get; set; }

	[Column("instruction_with_error")]
	public Guid InstructionWithError { get; set; }

	[Column("triggered_by")]
	public Guid TriggeredBy { get; set; }

	[Column("start_time", TypeName = "timespan(3) without time zone")]
	public DateTime StartTime { get; set; }

	[Column("duration", TypeName = "time without time zone")]
	public TimeSpan Duration { get; set; }

	[ForeignKey(nameof(PipelineId))]
	[InverseProperty(nameof(Postgres.Pipeline.PipelineLogs))]
	public virtual Pipeline Pipeline { get; set; } = null!;

	[ForeignKey(nameof(TriggeredBy))]
	[InverseProperty(nameof(User.PipelineLogTriggeredByNavigation))]
	public virtual User TriggeredByNavigation { get; set; } = null!;

	[ForeignKey(nameof(InstructionWithError))]
	[InverseProperty(nameof(Postgres.PipelineInstruction.PipelineLogs))]
	public virtual PipelineInstruction PipelineInstruction { get; set; } = null!;
}
