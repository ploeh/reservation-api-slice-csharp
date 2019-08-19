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
        public void TryAcceptReturnsReservationIdInHappyPathScenario()
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = 4
            };
            var td = new Mock<IReservationsRepository>();
            td
                .Setup(r => r.ReadReservations(reservation.Date))
                .Returns(new Reservation[0]);
            td.Setup(r => r.Create(reservation)).Returns(42);
            var sut = new MaîtreD(capacity: 10, td.Object);

            var actual = sut.TryAccept(reservation);

            Assert.Equal(42, actual);
        }
    }
}
