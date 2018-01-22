using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelReservation.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Common
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase()
        {
            _context = new HotelContext();
            _dbSet = _context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IList<Expression<Func<TEntity, object>>> includedProperties = null, int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> entities = _dbSet;
            //Filtering
            if (filter != null) entities = entities.Where(filter);

            //Sorting
            orderBy?.Invoke(entities);

            //Including
            if (includedProperties != null)
            {
                foreach (var property in includedProperties)
                    entities = entities.Include(property);
            }

            //Paging
            if (pageIndex.HasValue && pageSize.HasValue) entities = entities.Skip(pageSize.Value * pageIndex.Value).Take(pageSize.Value);

            return entities;
        }

        public virtual async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IList<Expression<Func<TEntity, object>>> includedProperties = null, int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> entities = _dbSet;
            //Filtering
            if (filter != null) entities = entities.Where(filter);

            //Sorting
            orderBy?.Invoke(entities);

            //Including
            if (includedProperties != null)
            {
                foreach (var property in includedProperties)
                    entities = entities.Include(property);
            }

            //Paging
            if (pageIndex.HasValue && pageSize.HasValue) entities = entities.Skip(pageSize.Value * pageIndex.Value).Take(pageSize.Value);

            return await entities.ToListAsync();
        }

        public virtual TEntity GetById(object id) => _dbSet.Find(id);

        public virtual Task<TEntity> GetAsyncById(object id) => _dbSet.FindAsync(id);

        public virtual TEntity Insert(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void BulkInsert(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> @where)
        {
            var entity = _dbSet.Find(where);
            if (entity != null)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                    _dbSet.Attach(entity);

                _dbSet.Remove(entity);
            }
        }

        public virtual void BulkDelete(IQueryable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
            }
            _dbSet.RemoveRange(entities.AsEnumerable());
        }

        public virtual void Detach(TEntity entity) => _context.Entry(entity).State = EntityState.Detached;

        public int SaveChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries<BaseEntity>())
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedOn = DateTime.Now;
                    entity.ModifiedOn = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                    entity.ModifiedOn = DateTime.Now;
            }
            return _context.SaveChanges();
        }

        public virtual void Dispose() => _context?.Dispose();
    }
}
