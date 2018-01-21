using System;
using System.Collections.Generic;
using HotelReservation.Bo;
using HotelReservation.Models;
using HotelReservation.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [Route("api/reservation")]
    public class ReservationController : Controller
    {
        BoReservation reservationBo = new BoReservation();
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]PrxReservation value)
        {
            var guest = new BoGuest().Create(value.GuestName, value.GuestEmail, value.GuestPhone);
            reservationBo.Book(guest, value.RoomNumber, value.ArrivalDate, value.DepartureDate);
        }

        [HttpPost("{id}/cancel")]
        public void Cancel(int id)
        {
        }

        [HttpPost("{id}/checkin")]
        public void CheckIn(int id)
        {
        }

        [HttpPost("{id}/checkout")]
        public void Checkout(int id, DateTime date)
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DtoReservation value)
        {
        }

    }
}
