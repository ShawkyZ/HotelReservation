using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Models.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Bo
{
    public class BoReservation : BoBase<DtoReservation>
    {
        public DtoReservation Book(DtoGuest dtoGuest, DtoRoom dtoRoom, DateTime arrivalData, DateTime departureDate)
        {
            // roles: 
            // number of days must be more than number of cancelation days fees
            if (dtoRoom == null)
                throw new ArgumentException("No room with the incoming number");
            if (arrivalData >= departureDate)
                throw new ArgumentException("Arrival date can't be greater than Departure Date");
           var dtoReservation = new DtoReservation
            {
                ArrivalDate = arrivalData,
                DepartureDate = departureDate,
                GuestId = dtoGuest.Id.Value,
                Room = dtoRoom
            };
            var deoositFees = dtoRoom.RoomType.DepositFeePercentage * (dtoRoom.RoomType.DepositFeePercentage / 100);
            dtoReservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.Booked, Fees = deoositFees });
            Repository.Insert(dtoReservation);
            Repository.SaveChanges();
            return dtoReservation;
        }
        public void Checkout(DtoReservation reservation)
        {
            // roles: 
            // mark reservation as checkedout
            // Hotel write down remaining amount to the Guest Account
            var reservationStatus =
                reservation.ReservationStatusList.SingleOrDefault(x => x.ReservationStatus == ReservationStatus.Booked);
            if (reservationStatus == null)
                throw new ArgumentException("Can't Checkout when a reservation isn't booked");
            var remainingFees = reservation.Room.RoomType.Rate - reservationStatus.Fees;
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.CheckedOut, Fees = remainingFees });
            Repository.Update(reservation);
            Repository.SaveChanges();
        }
        public void CheckIn(DtoReservation reservation)
        {
            //Marks that reservation as "Checked in"
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.CheckedIn });
            Repository.Update(reservation);
            Repository.SaveChanges();
        }
        public DtoReservation Get(Guid reservationId)
        {
            return Repository.Get(x=>x.Id == reservationId).Include(x=>x.ReservationStatusList).Include(x => x.Room).Include(x=>x.Room.RoomType).FirstOrDefault();
        }
        public void Cancel(DtoReservation reservation)
        {
            //hotel posts a cancellation fee to the Guest Account
            var cancelationFees = reservation.Room.RoomType.CancellationFeeNightsCount * reservation.Room.RoomType.Rate;
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.Canceled, Fees = cancelationFees });
            Repository.Update(reservation);
            Repository.SaveChanges();
        }

        public IEnumerable<DtoReservation> BrowseForReservationStatus(string reservationStatus)
        {
            return Repository.Get(x => x.GetCurrentStatus().ReservationStatus.ToString().ToLower() == reservationStatus.ToLower()).Include(x=>x.ReservationStatusList).ToList();
        }
        public IEnumerable<DtoReservation> SearchForReservations(DateTime? arrivalDateFrom = null, DateTime? arrivalDateTo = null, string guestFullName = null, string guestEmail = null, string guestPhoneNumber = null)
        {
            var filter = BuildReservationFilter(arrivalDateFrom, arrivalDateTo, guestFullName, guestEmail, guestPhoneNumber);

            return Repository.Get(filter).ToList();
        }
        public IEnumerable<DtoReservation> BrowseForGuest(Guid guestId)
        {
            return Repository.Get(x => x.Guest.Id == guestId).ToList();
        }

        public DtoReservation BrowseReservation(Guid reservationId)
        {
            //TODO: Return Guest Amounts
            return Repository.GetById(reservationId);
        }


        #region Private Methods
        private static ExpressionStarter<DtoReservation> BuildReservationFilter(DateTime? arrivalDateFrom = null, DateTime? arrivalDateTo = null, string guestFullName = null, string guestEmail = null, string guestPhoneNumber = null)
        {
            var predicate = PredicateBuilder.New<DtoReservation>();

            if (arrivalDateFrom.HasValue)
                predicate = predicate.And(r => r.ArrivalDate >= arrivalDateFrom.Value);

            if (arrivalDateTo.HasValue)
                predicate = predicate.And(r => r.ArrivalDate <= arrivalDateTo.Value);

            if (!string.IsNullOrEmpty(guestFullName))
                predicate = predicate.And(r => r.Guest.Name.ToLower().Contains(guestFullName.ToLower()));

            if (!string.IsNullOrEmpty(guestPhoneNumber))
                predicate = predicate.And(r => r.Guest.Phone.ToLower().Equals(guestPhoneNumber.ToLower()));

            if (!string.IsNullOrEmpty(guestEmail))
                predicate = predicate.And(r => r.Guest.Email.ToLower().Equals(guestEmail.ToLower()));

            return predicate;
        }
        #endregion

    }
}
