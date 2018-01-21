using System;
using NUnit.Framework;

namespace HotelReservation.Bo.Test
{
    [TestFixture]
    public class BoReservationTest
    {
        [TestCase(1)]
        [TestCase(2)]
        public void BookReservation_Test(int num)
        {
            Assert.AreEqual(1,num);
        }
    }
}
