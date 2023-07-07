using System.Linq.Expressions;

namespace Houston.Core.Interfaces.Repository {
	public interface IRepository<TEntity> where TEntity : class {
		Task<TEntity?> GetByIdAsync(Guid id);
		Task<IEnumerable<TEntity>> GetAllAsync();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expressions);
		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);
		void Update(TEntity entity);
		void UpdateRange(IEnumerable<TEntity> entities);
		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
	}
}
