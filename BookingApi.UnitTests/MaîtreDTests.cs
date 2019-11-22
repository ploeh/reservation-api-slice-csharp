/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class MaîtreDTests
    {
        [Theory]
        [InlineData("2018-08-30",  4, 10)]
        [InlineData("2019-09-29", 10, 10)]
        [InlineData("2020-10-28", 20, 20)]
        [InlineData("2021-11-27",  1, 22)]
        public void CanAcceptWhenCommunalTableHasEnoughCapacity(
            string date,
            int quantity,
            int capacity)
        {
            var reservation = new Reservation
            {
                Date = DateTime.Parse(date),
                Quantity = quantity
            };
            var sut = new MaîtreD(
                TimeSpan.FromHours(2),
                new Table(capacity));

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.True(actual);
        }

        [Theory]
        [InlineData( 4, 10)]
        [InlineData( 3,  3)]
        [InlineData(11, 14)]
        public void CanNotAcceptWhenCommunalTableHasInsufficientCapacity(
            int quantity,
            int capacity)
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = quantity
            };
            var sut = new MaîtreD(TimeSpan.FromHours(2), new Table(capacity));

            var actual = sut.CanAccept(
                new[] { new Reservation { Quantity = 7 } },
                reservation);

            Assert.False(actual);
        }

        [Theory]
        [InlineData( 1)]
        [InlineData( 9)]
        [InlineData(18)]
        public void RejectWhenRestaurantHasNoTables(int quantity)
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2019, 11, 21),
                Quantity = quantity
            };
            var sut = new MaîtreD(TimeSpan.FromHours(2));

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.False(actual);
        }

        [Theory]
        [InlineData( 3)]
        [InlineData( 8)]
        [InlineData(23)]
        public void AcceptWhenTableIsAvailable(int quantity)
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2019, 11, 20),
                Quantity = quantity
            };
            var sut = new MaîtreD(TimeSpan.FromHours(2), new Table(quantity));

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.True(actual);
        }

        [Theory]
        [InlineData( 3)]
        [InlineData(12)]
        [InlineData(33)]
        public void RejectReservationWhenTableIsAlreadyTaken(int quantity)
        {
            var dt = new DateTime(2019, 10, 3);
            var reservation = new Reservation
            {
                Date = dt,
                Quantity = quantity
            };
            var sut = new MaîtreD(TimeSpan.FromHours(2), new Table(quantity));

            var actual = sut.CanAccept(
                new[] { new Reservation { Date = dt, Quantity = quantity } },
                reservation);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(new[] { 1 })]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { 4, 4, 4, 4 })]
        public void RejectReservationOverTotalCapacity(int[] seats)
        {
            var dt = new DateTime(2017, 3, 8);
            var capacity = seats.Sum();
            var reservation = new Reservation
            {
                Date = dt,
                Quantity = capacity + 1
            };
            var sut = new MaîtreD(
                TimeSpan.FromHours(2),
                seats.Select(s => new Table(s)).ToArray());

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(new[] { 3 })]
        [InlineData(new[] { 3, 3, 4 })]
        [InlineData(new[] { 3, 2, 2, 3 })]
        public void RejectAnyReservationWhenSoldOut(int[] seats)
        {
            var dt = new DateTime(2018, 3, 2);
            var reservation = new Reservation
            {
                Date = dt,
                Quantity = 1
            };
            var sut = new MaîtreD(
                TimeSpan.FromHours(2),
                seats.Select(s => new Table(s)).ToArray());

            var actual = sut.CanAccept(
                seats.Select(s => new Reservation { Date = dt, Quantity = s }),
                reservation);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(new[] { 8, 4, 2, 2, 3 })]
        [InlineData(new[] { 22 })]
        [InlineData(new[] { 4, 2, 4, 3 })]
        public void RejectAnyReservationLargerThanTheBiggestTable(int[] seats)
        {
            var dt = new DateTime(2019, 10, 10);
            var biggestTable = seats.Max();
            var reservation = new Reservation
            {
                Date = dt,
                Quantity = biggestTable + 1
            };
            var sut = new MaîtreD(
                TimeSpan.FromHours(2),
                seats.Select(s => new Table(s)).ToArray());

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(new[] {    1, 1 },     new int[0], 2, false)]
        [InlineData(new[] {    2, 2 }, new[] {    2 }, 2,  true)]
        [InlineData(new[] {    3, 2 }, new[] {    3 }, 2,  true)]
        [InlineData(new[] {    2, 4 }, new[] {    3 }, 3, false)]
        [InlineData(new[] {    2, 4 }, new[] {    3 }, 2,  true)]
        [InlineData(new[] {    2, 4 }, new[] {    4 }, 3, false)]
        [InlineData(new[] {    2, 4 }, new[] {    4 }, 2,  true)]
        [InlineData(new[] {    2, 4 }, new[] {    2 }, 3,  true)]
        [InlineData(new[] {    4, 2 }, new[] {    3 }, 3, false)]
        [InlineData(new[] {    4, 2 }, new[] {    3 }, 2,  true)]
        [InlineData(new[] {    4, 2 }, new[] {    4 }, 3, false)]
        [InlineData(new[] {    4, 2 }, new[] {    4 }, 2,  true)]
        [InlineData(new[] {    4, 2 }, new[] {    2 }, 3,  true)]
        [InlineData(new[] {    4, 2 }, new[] { 2, 3 }, 1, false)]
        [InlineData(new[] { 4, 4, 2 }, new[] { 3, 2 }, 1,  true)]
        [InlineData(new[] { 4, 4, 2 }, new[] { 3, 2 }, 4,  true)]
        [InlineData(new[] { 4, 4, 2 }, new[] { 3, 2 }, 5, false)]
        public void CanAcceptReturnsCorrectResult(
            int[] seats,
            int[] reservations,
            int quantity,
            bool expected)
        {
            var dt = new DateTime(2019, 11, 21);
            var reservation = new Reservation
            {
                Date = dt,
                Quantity = quantity
            };
            var sut = new MaîtreD(
                TimeSpan.FromHours(2),
                seats.Select(s => new Table(s)).ToArray());

            var actual = sut.CanAccept(
                reservations
                    .Select(x => new Reservation { Date = dt, Quantity = x })
                    .ToList(),
                reservation);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(120,    new[] { 1 },    new[] { 1 }, 1)]
        [InlineData( 90, new[] { 2, 3 }, new[] { 2, 3 }, 2)]
        [InlineData( 90, new[] { 2, 3 }, new[] { 2, 3 }, 3)]
        [InlineData(100, new[] { 2, 3 }, new[] { 2, 2 }, 3)]
        public void AcceptWhenOtherReservationsStartASeatingDurationAfterReservation(
            double seatingDuration,
            int[] seats,
            int[] reservations,
            int quantity)
        {
            var dt = new DateTime(2019, 11, 22, 18, 0, 0);
            var reservation = new Reservation
            {
                Date = dt,
                Quantity = quantity
            };
            var sut = new MaîtreD(
                TimeSpan.FromMinutes(seatingDuration),
                seats.Select(s => new Table(s)).ToArray());

            var actual = sut.CanAccept(
                reservations.Select(x =>
                    new Reservation
                    {
                        Date = dt.AddMinutes(seatingDuration),
                        Quantity = x
                    }).ToArray(),
                reservation);

            Assert.True(actual);
        }
    }
}
