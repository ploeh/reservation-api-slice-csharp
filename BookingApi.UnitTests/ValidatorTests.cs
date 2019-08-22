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
        [Theory]
        [InlineData("2018-08-30")]
        [InlineData("2018-08-30T19:47:00")]
        [InlineData("2022-04-01 12:01:02")]
        public void ValidDate(string date)
        {
            var dto = new ReservationDto
            {
                Date = date
            };

            var actual = Validator.Validate(dto);

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("Invalid date")]
        [InlineData("foo")]
        [InlineData("  ")]
        public void InvalidDate(string date)
        {
            var dto = new ReservationDto
            {
                Date = date
            };

            var actual = Validator.Validate(dto);

            Assert.NotEmpty(actual);
        }
    }
}
