using Entities.Entities;
using System.Linq.Expressions;

namespace Abstractions.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Get(int id);

        Task<TEntity> GetAsync(int id);

        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity?> GetAll();

        TEntity? Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> InsertAndGetAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> predicate);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
