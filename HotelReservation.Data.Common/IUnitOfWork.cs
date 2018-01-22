using System;

namespace HotelReservation.Data.Common
{
    public interface IUnitOfWorkx : IDisposable
    {
        IRepository<T> RepositoryFor<T>() where T : class;
        int SaveChanges();
    }
}
