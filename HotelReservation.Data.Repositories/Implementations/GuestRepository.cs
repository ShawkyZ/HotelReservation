using HotelReservation.Data.Repositories.Interfaces;
using HotelReservation.Models.Entities;

namespace HotelReservation.Data.Repositories.Implementations
{
    public class GuestRepository : IGuestRepository
    {
        private Common.IUnitOfWork uf = new Common.UnitOfWork();

        public GuestRepository()
        {
            var repo = uf.RepositoryFor<DtoGuest>().GetById(1);
            uf.SaveChanges();
        }
    }
}
