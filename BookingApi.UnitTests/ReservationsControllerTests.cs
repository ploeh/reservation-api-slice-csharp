/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationsControllerTests
    {
        private readonly ITestOutputHelper testOutput;

        public ReservationsControllerTests(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;
        }

        [Theory]
        [InlineData(10)]
        [InlineData( 1)]
        [InlineData(99)]
        public void PostInvalidDto(int capacity)
        {
            var sut = new ReservationsController(
                TimeSpan.FromHours(2.5),
                new[] { new Table(capacity) },
                new FakeReservationsRepository(),
                new SystemClock());

            var dto = new ReservationDto { };
            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            var msg = Assert.IsAssignableFrom<string>(br.Value);
            Assert.NotEmpty(msg);
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData( 9, 9)]
        [InlineData(30, 4)]
        public void PostPastReservationWhenNoPriorReservationsExist(
            int capacity,
            int quantity)
        {
            var repository = new FakeReservationsRepository();
            var now = new DateTime(2019, 8, 21);
            var sut = new ReservationsController(
                TimeSpan.FromHours(2.5),
                new[] { new Table(capacity) },
                repository,
                new ConstantClock(now));

            var dto = new ReservationDto
            {
                Date = now.Subtract(TimeSpan.FromDays(1)).ToString(),
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            var msg = Assert.IsAssignableFrom<string>(br.Value);
            Assert.NotEmpty(msg);
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData( 9, 9)]
        [InlineData(30, 4)]
        public void PostValidDtoWhenNoPriorReservationsExist(
            int capacity,
            int quantity)
        {
            var repository = new FakeReservationsRepository();
            var now = new DateTime(2019, 9, 24);
            var sut = new ReservationsController(
                TimeSpan.FromHours(2.5),
                new[] { new Table(capacity) },
                repository,
                new ConstantClock(now));

            var dto = new ReservationDto
            {
                Date = now.AddDays(1).ToString(),
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            Assert.IsAssignableFrom<OkResult>(actual);
            Assert.NotEmpty(repository);
        }

        [Theory]
        [InlineData( 1,  2)]
        [InlineData( 1,  3)]
        [InlineData(11, 15)]
        public void PostValidDtoWhenSoldOut(int capacity, int quantity)
        {
            var repository = new FakeReservationsRepository();
            var now = new DateTime(2019, 9, 27);
            var sut = new ReservationsController(
                TimeSpan.FromHours(2.5),
                new[] { new Table(capacity) },
                repository,
                new ConstantClock(now));

            var dto = new ReservationDto
            {
                Date = now.AddDays(2).ToString(),
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, c.StatusCode);
        }
    }
}
