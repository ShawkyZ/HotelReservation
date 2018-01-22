using System;
using HotelReservation.Bo;
using HotelReservation.Models;
using HotelReservation.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelReservation.API.Controllers
{
    [Route("api/Reservation")]
    public class ReservationController : Controller
    {
        private readonly BoReservation _reservationBo;
        private readonly BoGuest _guestBo;
        private readonly BoRoom _roomBo;
        public ReservationController()
        {
            _reservationBo = new BoReservation();
            _guestBo = new BoGuest();
            _roomBo = new BoRoom();
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
        [HttpGet("status/{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return BadRequest("Reservation status can't be null");

                var result = _reservationBo.BrowseForReservationStatus(id);
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
                var room = _roomBo.Get(reservationModel.RoomNumber); 
                var reservation = _reservationBo.Book(guest, room, reservationModel.ArrivalDate, reservationModel.DepartureDate);
                reservationModel.Id = reservation.Id.ToString();
                return Ok(reservationModel);
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
                var reservation = _reservationBo.Get(id);
                _reservationBo.Cancel(reservation);
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
                var reservation = _reservationBo.Get(id);
                _reservationBo.CheckIn(reservation);
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
                var reservation = _reservationBo.Get(id);
                _reservationBo.Checkout(reservation);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}
