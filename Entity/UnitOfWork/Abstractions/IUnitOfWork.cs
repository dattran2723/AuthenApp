using Entities.Entities;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Abstractions.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        AuthDbContext Context { get; }

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        int Complete();
    }
}
