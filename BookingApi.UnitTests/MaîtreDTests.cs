/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class MaîtreDTests
    {
        [Fact]
        public void CanAcceptReturnsReservationInHappyPathScenario()
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = 4
            };
            var td = new Mock<IReservationsRepository>();
            var sut = new MaîtreD(capacity: 10, td.Object);

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.True(actual);
        }

        [Fact]
        public void CanAcceptOnInsufficientCapacity()
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = 4
            };
            var td = new Mock<IReservationsRepository>();
            var sut = new MaîtreD(capacity: 10, td.Object);

            var actual = sut.CanAccept(
                new[] { new Reservation { Quantity = 7 } },
                reservation);

            Assert.False(actual);
            td.Verify(r => r.Create(It.IsAny<Reservation>()), Times.Never);
        }
    }
}
