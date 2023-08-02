namespace Houston.Infrastructure.Repository {
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
		protected readonly PostgresContext Context;
		protected readonly DbSet<TEntity> DbSet;

		protected Repository(PostgresContext context) {
			Context = context;
			DbSet = Context.Set<TEntity>();
		}

		public void Add(TEntity entity) {
			DbSet.Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> entities) {
			DbSet.AddRange(entities);
		}

		public void RemoveRange(IEnumerable<TEntity> entities) {
			DbSet.RemoveRange(entities);
		}

		public void UpdateRange(IEnumerable<TEntity> entities) {
			DbSet.UpdateRange(entities);
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expressions) {
			return DbSet.Where(expressions);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync() {
			return await DbSet.ToListAsync();
		}

		public async Task<TEntity?> GetByIdAsync(Guid id) {
			return await DbSet.FindAsync(id);
		}

		public void Remove(TEntity entity) {
			DbSet.Remove(entity);
		}

		public void Update(TEntity entity) {
			DbSet.Attach(entity);
			Context.Entry(entity).State = EntityState.Modified;
			DbSet.Update(entity);
		}
	}
}
