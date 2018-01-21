using HotelReservation.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Common
{
    public class HotelContext : DbContext
    {
        public HotelContext()
        {
        }

        public HotelContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DtoGuest> Guests { get; set; }
        public DbSet<DtoReservation> Reservations { get; set; }
        public DbSet<DtoRoom> Rooms { get; set; }
        public DbSet<DtoRoomType> RoomTypes { get; set; }
        public DbSet<DtoReservationStatus> ReservationStatuses { get; set; }
    }
}
