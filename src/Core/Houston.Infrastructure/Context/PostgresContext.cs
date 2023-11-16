namespace Houston.Infrastructure.Context;

public partial class PostgresContext : DbContext {
	public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }

	public virtual DbSet<Connector> Connector { get; set; }

	public virtual DbSet<ConnectorFunction> ConnectorFunction { get; set; }

	public virtual DbSet<Pipeline> Pipeline { get; set; }

	public virtual DbSet<PipelineTrigger> PipelineTrigger { get; set; }

	public virtual DbSet<User> User { get; set; }

	public virtual DbSet<PipelineLog> PipelineLog { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.ApplyConfiguration(new ConnectorConfiguration());

		modelBuilder.ApplyConfiguration(new ConnectorFunctionConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineLogConfiguration());

		modelBuilder.ApplyConfiguration(new PipelineTriggerConfiguration());

		modelBuilder.ApplyConfiguration(new UserConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
