using Abstractions.UnitOfWork;
using Entities.Entities;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Entity.UnitOfWork
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public AuthDbContext Context;

        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        public Repository(AuthDbContext context)
        {
            Context = context;
        }

        public TEntity Get(int id)
        {
            var entity = GetAll().FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new Exception("Not Found");
            }

            return entity;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var entity = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new Exception("Not Found");
            }

            return entity;
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }


        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public async Task<TEntity> InsertAndGetAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            Context.SaveChanges();

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.FromResult(0);
        }
    }
}
