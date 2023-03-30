using Houston.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("User", Schema = "houston")]
[Index("Email", Name = "User_email_uq", IsUnique = true)]
public partial class User {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("name", TypeName = "character varying")]
	public string Name { get; set; } = null!;

	[Column("email", TypeName = "character varying")]
	public string Email { get; set; } = null!;

	[Column("password")]
	[StringLength(256)]
	public string Password { get; set; } = null!;

	[Column("role")]
	public UserRoleEnum Role { get; set; }

	[Required]
	[Column("active")]
	public bool Active { get; set; }

	[Required]
	[Column("first_access")]
	public bool FirstAccess { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) without time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp without time zone")]
	public DateTime LastUpdate { get; set; }

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<Connector> ConnectorCreatedByNavigation { get; } = new List<Connector>();

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<ConnectorFunction> ConnectorFunctionCreatedByNavigation { get; } = new List<ConnectorFunction>();

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<ConnectorFunctionInput> ConnectorFunctionInputCreatedByNavigation { get; } = new List<ConnectorFunctionInput>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<ConnectorFunctionInput> ConnectorFunctionInputUpdatedByNavigation { get; } = new List<ConnectorFunctionInput>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<ConnectorFunction> ConnectorFunctionUpdatedByNavigation { get; } = new List<ConnectorFunction>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<Connector> ConnectorUpdatedByNavigation { get; } = new List<Connector>();

	[ForeignKey("CreatedBy")]
	[InverseProperty("InverseCreatedByNavigation")]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<User> InverseCreatedByNavigation { get; } = new List<User>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<User> InverseUpdatedByNavigation { get; } = new List<User>();

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<Pipeline> PipelineCreatedByNavigation { get; } = new List<Pipeline>();

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<PipelineInstruction> PipelineInstructionCreatedByNavigation { get; } = new List<PipelineInstruction>();

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInputCreatedByNavigation { get; } = new List<PipelineInstructionInput>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInputUpdatedByNavigation { get; } = new List<PipelineInstructionInput>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<PipelineInstruction> PipelineInstructionUpdatedByNavigation { get; } = new List<PipelineInstruction>();

	[InverseProperty("CreatedByNavigation")]
	public virtual ICollection<PipelineTrigger> PipelineTriggerCreatedByNavigation { get; } = new List<PipelineTrigger>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<PipelineTrigger> PipelineTriggerUpdatedByNavigation { get; } = new List<PipelineTrigger>();

	[InverseProperty("UpdatedByNavigation")]
	public virtual ICollection<Pipeline> PipelineUpdatedByNavigation { get; } = new List<Pipeline>();

	[InverseProperty("TriggeredByNavigation")]
	public virtual ICollection<PipelineLog> PipelineLogTriggeredByNavigation { get; } = new List<PipelineLog>();

	[ForeignKey("UpdatedBy")]
	[InverseProperty("InverseUpdatedByNavigation")]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
