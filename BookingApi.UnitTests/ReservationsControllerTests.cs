/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationsControllerTests
    {
        [Theory]
        [InlineData(10)]
        [InlineData( 1)]
        [InlineData(99)]
        public void PostInvalidDto(int capacity)
        {
            var sut = new ReservationsController(
                new FakeReservationsRepository(),
                capacity);

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
        public void PostValidDtoWhenNoPriorReservationsExist(
            int capacity,
            int quantity)
        {
            var repository = new FakeReservationsRepository();
            var sut = new ReservationsController(repository, capacity);

            var dto = new ReservationDto
            {
                Date = "2019-08-20",
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            Assert.IsAssignableFrom<OkObjectResult>(actual);
            Assert.NotEmpty(repository);
        }

        [Theory]
        [InlineData( 1,  2)]
        [InlineData( 1,  3)]
        [InlineData(11, 15)]
        public void PostValidDtoWhenSoldOut(int capacity, int quantity)
        {
            var repository = new FakeReservationsRepository();
            var sut = new ReservationsController(repository, capacity);

            var dto = new ReservationDto
            {
                Date = "2019-08-20",
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, c.StatusCode);
        }
    }
}
