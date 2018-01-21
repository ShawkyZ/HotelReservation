using HotelReservation.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Common
{
    public class HotelContext : DbContext
    {
        public HotelContext(): base(new DbContextOptionsBuilder().UseSqlServer("Password=123;Persist Security Info=True;User ID=task;Initial Catalog=taskDB;Data Source=52.178.217.7").Options)
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
