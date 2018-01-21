using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Models.Entities;
using LinqKit;

namespace HotelReservation.Bo
{
    public class BoReservation : BoBase<DtoReservation>
    {
        public DtoReservation Book(DtoGuest dtoGuest, string roomNumber, DateTime arrivalData, DateTime departureDate)
        {
            // roles: 
            // number of days must be more than number of cancelation days fees
            var dtoRoom = UnitOfWork.RepositoryFor<DtoRoom>().Get(x => x.Number == roomNumber).SingleOrDefault();
            if (dtoRoom == null)
                throw new ArgumentException("No room with the incoming number");
            if (arrivalData >= departureDate)
                throw new ArgumentException("Arrival date can't be greater than Departure Date");
            if ((departureDate - arrivalData).Days < dtoRoom.RoomType.CancellationFeeNightsCount)
                throw new ArgumentException("number of days must be more than number of cancelation days fees");
            var dtoReservation = new DtoReservation
            {
                ArrivalDate = arrivalData,
                DepartureDate = departureDate,
                Guest = dtoGuest,
                Room = dtoRoom
            };
            var deoositFees = 100 / (dtoReservation.Room.RoomType.DepositFeePercentage);
            dtoReservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.Booked, Fees = deoositFees });
            Repository.Insert(dtoReservation);
            UnitOfWork.SaveChanges();
            return dtoReservation;
        }
        public void Checkout(Guid reservationId)
        {
            // roles: 
            // mark reservation as checkedout
            // Hotel write down remaining amount to the Guest Account
            var reservation = Repository.GetById(reservationId);
            var reservationStatus =
                reservation.ReservationStatusList.SingleOrDefault(x => x.ReservationStatus == ReservationStatus.Booked);
            if (reservationStatus == null)
                throw new ArgumentException("Can't Checkout when a reservation isn't booked");
            var remainingFees = reservation.Room.RoomType.Rate - reservationStatus.Fees;
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.CheckedOut, Fees = remainingFees });
            Repository.Update(reservation);
            UnitOfWork.SaveChanges();
        }
        public void CheckIn(Guid reservationId)
        {
            //Marks that reservation as "Checked in"
            var reservation = Repository.GetById(reservationId);
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.CheckedIn });
            Repository.Update(reservation);
            UnitOfWork.SaveChanges();
        }

        public void Cancel(Guid reservationId)
        {
            //hotel posts a cancellation fee to the Guest Account
            var reservation = Repository.GetById(reservationId);
            var cancelationFees = reservation.Room.RoomType.CancellationFeeNightsCount * reservation.Room.RoomType.Rate;
            reservation.AddReservationStatus(new DtoReservationStatus { ReservationStatus = ReservationStatus.Canceled, Fees = cancelationFees });
            Repository.Update(reservation);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<DtoReservation> BrowseForReservationStatus(string reservationStatus)
        {
            return Repository.Get(x => x.GetCurrentStatus().ReservationStatus.ToString().ToLower() == reservationStatus.ToLower()).ToList();
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
            var predicate = PredicateBuilder.True<DtoReservation>();

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
