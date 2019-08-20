/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static Ploeh.Samples.BookingApi.UnitTests.Resemblance;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationsControllerTests
    {
        [Fact]
        public void PostInvalidDto()
        {
            var sut = new ReservationsController(
                new FakeReservationsRepository(),
                10);

            var dto = new ReservationDto { };
            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            var msg = Assert.IsAssignableFrom<string>(br.Value);
            Assert.NotEmpty(msg);
        }

        [Fact]
        public void PostValidDtoWhenNoPriorReservationsExist()
        {
            var repository = new FakeReservationsRepository();
            var sut = new ReservationsController(repository, 10);

            var dto = new ReservationDto { Date = "2019-08-20", Quantity = 1 };
            var actual = sut.Post(dto);

            Assert.IsAssignableFrom<OkObjectResult>(actual);
            Assert.NotEmpty(repository);
        }

        [Fact]
        public void PostValidDtoWhenSoldOut()
        {
            var repository = new FakeReservationsRepository();
            var sut = new ReservationsController(repository, 1);

            var dto = new ReservationDto { Date = "2019-08-20", Quantity = 2 };
            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, c.StatusCode);
        }
    }
}
