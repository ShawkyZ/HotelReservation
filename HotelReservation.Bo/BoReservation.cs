using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Models.Entities;

namespace HotelReservation.Bo
{
    public class BoReservation : BoBase<DtoReservation>
    {
        public DtoReservation Book(DtoGuest dtoGuest, string roomNumber, DateTime arrivalData, DateTime departureDate)
        {
            // roles: 
            // number of days must be more than number of cancelation days fees
            var dtoRoom = UnitOfWork.RepositoryFor<DtoRoom>().Get(x => x.Number == roomNumber).SingleOrDefault();
            if(dtoRoom == null)
                throw new ArgumentException("No room with the incoming number");
            if(arrivalData >= departureDate)
                throw new ArgumentException("Arrival date can't be greater than Departure Date");
            var dtoReservation = new DtoReservation
            {
                ArrivalDate = arrivalData,
                DepartureDate = departureDate,
                Guest = dtoGuest,
                Room = dtoRoom
            };
            var deoositFees =  100 / (dtoReservation.Room.RoomType.DepositFeePercentage);
            dtoReservation.AddReservationStatus(new DtoReservationStatus{ReservationStatus = ReservationStatus.Booked, Fees = deoositFees});
            Repository.Insert(dtoReservation);
            UnitOfWork.SaveChanges();
            return dtoReservation;
        }
        public void Checkout(DtoReservation reservation)
        {
            // roles: 
            // mark reservation as checkedout
            // Hotel write down remaining amount to the Guest Account
            var reservationStatus =
                reservation.ReservationStatusList.SingleOrDefault(x => x.ReservationStatus == ReservationStatus.Booked);
            if(reservationStatus == null)
                throw new ArgumentException("Can't Checkout when a reservation isn't booked");
            var remainingFees = reservation.Room.RoomType.Rate - reservationStatus.Fees;
            reservation.AddReservationStatus(new DtoReservationStatus{ReservationStatus = ReservationStatus.CheckedOut, Fees = remainingFees});
            Repository.Update(reservation);
        }
        public void CheckIn(DtoReservation reservation)
        {
            //Marks that reservation as "Checked in"
            reservation.AddReservationStatus(new DtoReservationStatus{ReservationStatus = ReservationStatus.CheckedIn});
            Repository.Update(reservation);
        }

        public void Cancel(DtoReservation reservation)
        {
            //hotel posts a cancellation fee to the Guest Account
            var cancelationFees = reservation.Room.RoomType.CancellationFeeNightsCount * reservation.Room.RoomType.Rate;
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.Canceled, Fees = cancelationFees});
            Repository.Update(reservation);
        }

        
        /* Browse reservation dummy logic
        public IEnumerable<DtoReservation> Browse(DtoReservationStatus reservationStatus)
        {
           
        }
        public IEnumerable<DtoReservation> Browse(DateTime arrivalDateFrom)
        {

        }
        public IEnumerable<DtoReservation> Browse(DateTime arrivalDateTo)
        {

        }
        public IEnumerable<DtoReservation> Browse(DtoGuest guest)
        {

        }
        */
    }
}
