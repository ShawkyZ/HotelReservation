using HotelReservation.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelReservation.Data.Common
{
    public class UnitOfWorkx : IUnitOfWorkx
    {
        #region Fields

        private readonly DbContext _context;

        #endregion

        #region Constructor
        public UnitOfWorkx()
        {
            DbContext context = new HotelContext();
            if (context == null) throw new Exception("Context cannot be null");

            _context = context;
        }

        #endregion

        #region Interface Implementation

        public IRepository<TEntity> RepositoryFor<TEntity>() where TEntity : class
        {
            return new RepositoryBase<TEntity>();
        }


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

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
