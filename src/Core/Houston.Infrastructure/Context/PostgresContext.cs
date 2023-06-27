using Houston.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
namespace Houston.Infrastructure.Context;

public partial class PostgresContext : DbContext {
	public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }

	public virtual DbSet<Connector> Connector { get; set; }

	public virtual DbSet<ConnectorFunction> ConnectorFunction { get; set; }

	public virtual DbSet<ConnectorFunctionInput> ConnectorFunctionInput { get; set; }

	public virtual DbSet<Pipeline> Pipeline { get; set; }

	public virtual DbSet<PipelineInstruction> PipelineInstruction { get; set; }

	public virtual DbSet<PipelineInstructionInput> PipelineInstructionInput { get; set; }

	public virtual DbSet<PipelineTrigger> PipelineTrigger { get; set; }

	public virtual DbSet<PipelineTriggerEvent> PipelineTriggerEvent { get; set; }

	public virtual DbSet<PipelineTriggerFilter> PipelineTriggerFilter { get; set; }

	public virtual DbSet<TriggerEvent> TriggerEvent { get; set; }

	public virtual DbSet<TriggerFilter> TriggerFilter { get; set; }

	public virtual DbSet<User> User { get; set; }

	public virtual DbSet<PipelineLog> PipelineLog { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Connector>(entity => {
			entity.HasKey(e => e.Id).HasName("Connector_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_created_by");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_updated_by");
		});

		modelBuilder.Entity<ConnectorFunction>(entity => {
			entity.HasKey(e => e.Id).HasName("ConnectorFunction_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.Connector).WithMany(p => p.ConnectorFunction)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Connector_id_connector_id_fk");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorFunctionCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorFunctionUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		});

		modelBuilder.Entity<ConnectorFunctionInput>(entity => {
			entity.HasKey(e => e.Id).HasName("ConnectorFunctionInput_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.ConnectorFunction).WithMany(p => p.ConnectorFunctionInputs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunction_id_connector_function_id_fk");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorFunctionInputCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorFunctionInputUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		});

		modelBuilder.Entity<Pipeline>(entity => {
			entity.HasKey(e => e.Id).HasName("Pipeline_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		});

		modelBuilder.Entity<PipelineInstruction>(entity => {
			entity.HasKey(e => e.Id).HasName("PipelineInstruction_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.ConnectionNavigation).WithMany(p => p.InverseConnectionNavigation).HasConstraintName("PipelineInstruction_id_connection_fk");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineInstructionCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_id");

			entity.HasOne(d => d.Pipeline).WithMany(p => p.PipelineInstructions)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineInstructionUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");

			entity.HasOne(d => d.ConnectorFunction).WithMany(p => p.PipelineInstructions)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("PipelineInstruction_connector_function_id_fk");
		});

		modelBuilder.Entity<PipelineInstructionInput>(entity => {
			entity.HasKey(e => e.Id).HasName("PipelineInstructionInput_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineInstructionInputCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.ConnectorFunctionInput).WithMany(p => p.PipelineInstructionInputs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunctionInput_id_input_id_fk");

			entity.HasOne(d => d.PipelineInstruction).WithMany(p => p.PipelineInstructionInputs)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineInstruction_id_input_id_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineInstructionInputUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		});

		modelBuilder.Entity<PipelineTrigger>(entity => {
			entity.HasKey(e => e.Id).HasName("PipelineTrigger_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineTriggerCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.Pipeline).WithOne(p => p.PipelineTrigger)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineTriggerUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		});

		modelBuilder.Entity<PipelineTriggerEvent>(entity => {
			entity.HasKey(e => e.Id).HasName("PipelineTriggerEvent_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.PipelineTrigger).WithMany(p => p.PipelineTriggerEvents)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerEvent_pipeline_trigger_id_fk");

			entity.HasOne(d => d.TriggerEvent).WithMany(p => p.PipelineTriggerEvents)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerEvent_trigger_event_id_fk");
		});

		modelBuilder.Entity<PipelineTriggerFilter>(entity => {
			entity.HasKey(e => e.Id).HasName("PipelineTriggerFilter_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.PipelineTriggerEvent).WithMany(p => p.PipelineTriggerFilters)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerFilter_pipeline_trigger_event_id_fk");

			entity.HasOne(d => d.TriggerFilter).WithMany(p => p.PipelineTriggerFilters)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerFilter_trigger_filter_id_fk");
		});

		modelBuilder.Entity<TriggerEvent>(entity => {
			entity.HasKey(e => e.Id).HasName("TriggerEvent_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();
		});

		modelBuilder.Entity<TriggerFilter>(entity => {
			entity.HasKey(e => e.Id).HasName("TriggerFilter_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();
		});

		modelBuilder.Entity<User>(entity => {
			entity.HasKey(e => e.Id).HasName("User_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_created_by");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_updated_by");
		});

		modelBuilder.Entity<PipelineLog>(entity => {
			entity.HasKey(e => e.Id).HasName("PipelineLog_pk");

			entity.Property(e => e.Id).ValueGeneratedNever();

			entity.HasOne(d => d.Pipeline).WithMany(p => p.PipelineLogs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			entity.HasOne(d => d.TriggeredByNavigation).WithMany(p => p.PipelineLogTriggeredByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_triggered_by_fk");

			entity.HasOne(d => d.PipelineInstruction).WithMany(p => p.PipelineLogs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("PipelineInstruction_id_instruction_with_error");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
