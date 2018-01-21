using System;
using HotelReservation.Bo;
using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [Route("api/Reservation")]
    public class ReservationController : Controller
    {
        private readonly BoReservation _reservationBo;
        private readonly BoGuest _guestBo;
        public ReservationController()
        {
            _reservationBo = new BoReservation();
            _guestBo = new BoGuest();
        }

        // GET api/Reservation
        [HttpGet]
        public IActionResult Get(DateTime? arrivalDateFrom = null, DateTime? arrivalDateTo = null, string guestFullName = null, string guestEmail = null, string guestPhoneNumber = null)
        {
            try
            {
                var result = _reservationBo.SearchForReservations(arrivalDateFrom, arrivalDateTo, guestFullName, guestEmail, guestPhoneNumber);

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // GET api/Reservation/{reservationStatus}
        [HttpGet("{reservationStatus}")]
        public IActionResult Get(string reservationStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(reservationStatus)) return BadRequest("Reservation status can't be null");

                var result = _reservationBo.BrowseForReservationStatus(reservationStatus);
                return Ok(result);

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // GET api/Reservation/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _reservationBo.BrowseReservation(id);
                return Ok(result);

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // POST api/Reservation
        [HttpPost]
        public IActionResult Post([FromBody]PrxReservation reservationModel)
        {
            try
            {
                var guest = _guestBo.Create(reservationModel.GuestName, reservationModel.GuestEmail, reservationModel.GuestPhone);
                var reservation = _reservationBo.Book(guest, reservationModel.RoomNumber, reservationModel.ArrivalDate, reservationModel.DepartureDate);

                return Ok(reservation);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // PUT api/Reservation/{id}/Cancel
        [HttpPut("{id}/Cancel")]
        public IActionResult Cancel(Guid id)
        {
            try
            {
                _reservationBo.Cancel(id);
                return Ok();

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // PUT api/Reservation/{id}/Checkin
        [HttpPut("{id}/Checkin")]
        public IActionResult CheckIn(Guid id)
        {
            try
            {
                _reservationBo.CheckIn(id);
                return Ok();

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // PUT api/Reservation/{id}/Checkout
        [HttpPut("{id}/Checkout")]
        public IActionResult Checkout(Guid id)
        {
            try
            {
                _reservationBo.Checkout(id);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}
