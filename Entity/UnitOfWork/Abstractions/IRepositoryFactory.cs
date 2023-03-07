using Entities.Entities;

namespace Abstractions.UnitOfWork
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
    }
}
