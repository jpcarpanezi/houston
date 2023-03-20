using Houston.Core.Entities.MongoDB;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Houston.Core.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase {
		Task InsertOneAsync(TEntity obj);

		Task<TEntity> FindByIdAsync(ObjectId id);

		Task ReplaceOneAsync(TEntity document);

		Task UpdateOneAsync<TEntityFilter, TEntityUpdate>(Expression<Func<TEntity, TEntityFilter>> filterExp, TEntityFilter filterValue, Expression<Func<TEntity, TEntityUpdate>> updateExp, TEntityUpdate updateValue);

		Task DeleteOneAsync(Guid id);
	}
}
