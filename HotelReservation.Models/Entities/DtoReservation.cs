using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservation.Models.Entities
{
    public class DtoReservation : BaseEntity
    {
        public DtoGuest Guest { get; set; }
        public Guid GuestId { get; set; }
        public DtoRoom Room { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public ICollection<DtoReservationStatus> ReservationStatusList { get; set; }

        public void AddReservationStatus(DtoReservationStatus reservationStatus)
        {
            if(ReservationStatusList == null)
                ReservationStatusList = new List<DtoReservationStatus>();
            if (ReservationStatusList.Contains(reservationStatus))
                return;
            ReservationStatusList.Add(reservationStatus);
        }

        public DtoReservationStatus GetCurrentStatus()
        {
            return ReservationStatusList.OrderByDescending(x => x.CreatedOn).First();
        }
    }
}