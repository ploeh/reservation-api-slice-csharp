/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ValidatorTests
    {
        [Fact]
        public void ValidDate()
        {
            var dto = new ReservationDto
            {
                Date = "2018-08-30T19:47:00"
            };

            var actual = Validator.Validate(dto);

            Assert.Empty(actual);
        }

        [Fact]
        public void InvalidDate()
        {
            var dto = new ReservationDto
            {
                Date = "Invalid date"
            };

            var actual = Validator.Validate(dto);

            Assert.NotEmpty(actual);
        }
    }
}
