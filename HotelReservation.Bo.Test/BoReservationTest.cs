using System;
using System.Linq;
using HotelReservation.Data.Common;
using HotelReservation.Data.Repositories.Interfaces;
using HotelReservation.Models.Entities;
using Moq;
using NUnit.Framework;
using Unity.Lifetime;
using Unity.Registration;

namespace HotelReservation.Bo.Test
{
    [TestFixture]
    public class BoReservationTest
    {
        [SetUp]
        public void Setup()
        {
            //UnityConfiguration.RegisterTypes();
            var reservationRepositoryMocked = new Mock<IReservationRepository>();
            UnityConfiguration.Container.RegisterInstance(typeof(IRepository<DtoReservation>), "", reservationRepositoryMocked.Object, new PerThreadLifetimeManager());
            reservationRepositoryMocked.Setup(x => x.Insert(It.IsAny<DtoReservation>())).Returns<DtoReservation>(x => x);
        }

        [Test]
        public void BookReservation_Test()
        {
            //Arrange
            var boReservation = new BoReservation();
            
            //Act
            var dtoReservation = boReservation.Book(new DtoGuest(), new DtoRoom {RoomType = new DtoRoomType()}, DateTime.Now,
                DateTime.Now.AddDays(1));
            
            //Assert
            Assert.IsTrue(dtoReservation.ReservationStatusList.Select(x=>x.ReservationStatus).Contains(ReservationStatus.Booked));
        }
    }
}
