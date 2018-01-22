using System;

namespace HotelReservation.Data.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> RepositoryFor<T>() where T : class;
        int SaveChanges();
    }
}
