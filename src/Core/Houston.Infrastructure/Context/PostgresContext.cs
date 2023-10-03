namespace Houston.Infrastructure.Context;

public partial class PostgresContext : DbContext {
	public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }

	public virtual DbSet<Connector> Connector { get; set; }

	public virtual DbSet<ConnectorFunction> ConnectorFunction { get; set; }

	public virtual DbSet<ConnectorFunctionHistory> ConnectorFunctionHistory { get; set; }

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
		modelBuilder.ApplyConfiguration(new ConnectorConfiguration());

		modelBuilder.ApplyConfiguration(new ConnectorFunctionConfiguration());

		modelBuilder.ApplyConfiguration(new ConnectorFunctionHistoryConfiguration());

		modelBuilder.ApplyConfiguration(new ConnectorFunctionInputConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineInstructionConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineInstructionInputConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineTriggerConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineTriggerEventConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineTriggerFilterConfiguration());

		modelBuilder.ApplyConfiguration(new TriggerEventConfiguration());

		modelBuilder.ApplyConfiguration(new TriggerFilterConfiguration());

		modelBuilder.ApplyConfiguration(new UserConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineLogConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
