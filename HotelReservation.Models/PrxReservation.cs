using System;

namespace HotelReservation.Models
{
    public class PrxReservation
    {
        public string Id { get; set; }
        public string GuestName { get; set; }
        public string GuestPhone { get; set; }
        public string GuestEmail { get; set; }
        public string RoomNumber { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
