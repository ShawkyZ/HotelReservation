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
        private Mock<IUnitOfWork> unitOfWorkMocked;
        [SetUp]
        public void Setup()
        {
            unitOfWorkMocked = new Mock<IUnitOfWork>();
            UnityConfiguration.Container.RegisterInstance(typeof(IUnitOfWork), "", unitOfWorkMocked.Object, new PerThreadLifetimeManager());
        }

        [Test]
        public void BookReservation_Test()
        {
            //Arrange
            unitOfWorkMocked.Setup(x => x.RepositoryFor<DtoReservation>().Insert(It.IsAny<DtoReservation>())).Returns<DtoReservation>(x => x);
            var boReservation = new BoReservation();

            //Act
            var dtoReservation = boReservation.Book(new DtoGuest(), new DtoRoom { RoomType = new DtoRoomType() }, DateTime.Now,
                DateTime.Now.AddDays(1));

            //Assert
            Assert.IsTrue(dtoReservation.ReservationStatusList.Select(x => x.ReservationStatus).Contains(ReservationStatus.Booked));
        }
    }
}
