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

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationsControllerTests
    {
        [Fact]
        public void PostInvalidDto()
        {
            var dto = new ReservationDto { };
            var validatorTD = new Mock<IValidator>();
            var sut = new ReservationsController(
                validatorTD.Object,
                new Mock<IMapper>().Object,
                new Mock<IReservationsRepository>().Object,
                10);

            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            var msg = Assert.IsAssignableFrom<string>(br.Value);
            Assert.NotEmpty(msg);
        }

        [Fact]
        public void PostValidDtoWhenNoPriorReservationsExist()
        {
            var dto = new ReservationDto { Date = "2019-08-20" };
            var r = new Reservation { };
            var validatorTD = new Mock<IValidator>();
            var mapperTD = new Mock<IMapper>();
            mapperTD.Setup(m => m.Map(dto)).Returns(r);
            var repositoryTD = new Mock<IReservationsRepository>();
            repositoryTD.Setup(repo => repo.Create(r)).Returns(1337);
            var sut = new ReservationsController(
                validatorTD.Object,
                mapperTD.Object,
                repositoryTD.Object,
                10);

            var actual = sut.Post(dto);

            var ok = Assert.IsAssignableFrom<OkObjectResult>(actual);
            Assert.Equal(1337, ok.Value);
        }

        [Fact]
        public void PostValidDtoWhenSoldOut()
        {
            var dto = new ReservationDto { Date = "2019-08-20" };
            var r = new Reservation { Quantity = 2 };
            var validatorTD = new Mock<IValidator>();
            var mapperTD = new Mock<IMapper>();
            mapperTD.Setup(m => m.Map(dto)).Returns(r);
            var sut = new ReservationsController(
                validatorTD.Object,
                mapperTD.Object,
                new Mock<IReservationsRepository>().Object,
                1);

            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, c.StatusCode);
        }
    }
}
