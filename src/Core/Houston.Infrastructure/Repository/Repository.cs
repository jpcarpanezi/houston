using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Houston.Infrastructure.Repository
{
    public class Repository<TEntity> : EntityBase, IRepository<TEntity> where TEntity : EntityBase {
		protected readonly IMongoContext Context;
		protected IMongoCollection<TEntity> DbSet;

		protected Repository(IMongoContext context) {
			Context = context;
			DbSet = Context.GetCollection<TEntity>(typeof(TEntity));
		}

		public async virtual Task InsertOneAsync(TEntity obj) {
			await DbSet.InsertOneAsync(obj);
		}

		public virtual async Task<TEntity?> FindByIdAsync(ObjectId id) {
			var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
			return data.FirstOrDefault();
		}

		public virtual async Task ReplaceOneAsync(TEntity document) {
			var filter = Builders<TEntity>.Filter.Eq("_id", document.Id);
			await DbSet.ReplaceOneAsync(filter, document);
		}

		public virtual async Task UpdateOneAsync<TEntityFilter, TEntityUpdate>(Expression<Func<TEntity, TEntityFilter>> filterExp, TEntityFilter filterValue, Expression<Func<TEntity, TEntityUpdate>> updateExp, TEntityUpdate updateValue) {
			var filter = Builders<TEntity>.Filter.Eq(filterExp, filterValue);
			var update = Builders<TEntity>.Update.Set(updateExp, updateValue);

			await DbSet.UpdateOneAsync(filter, update);
		}

		public async virtual Task DeleteOneAsync(Guid id) {
			await DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));
		}
	}
}
