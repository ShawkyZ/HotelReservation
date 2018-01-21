using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelReservation.Data.Common
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void BulkDelete(IQueryable<TEntity> entities);
        void BulkInsert(IEnumerable<TEntity> entities);
        void Delete(Expression<Func<TEntity, bool>> where);
        void Detach(TEntity entity);
        void Dispose();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IList<Expression<Func<TEntity, object>>> includedProperties = null, int? pageIndex = null, int? pageSize = null);
        Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IList<Expression<Func<TEntity, object>>> includedProperties = null, int? pageIndex = null, int? pageSize = null);
        Task<TEntity> GetAsyncById(object id);
        TEntity GetById(object id);
        object Insert(TEntity entity);
        void Update(TEntity entity);
    }
}