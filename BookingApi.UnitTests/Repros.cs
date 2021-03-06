﻿/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class Repros
    {
        [Fact]
        public void CapacityEdgeCase()
        {
            var log = Log.LoadEmbeddedFile("CapacityEdgeCase.txt");
            var (sut, dto) = Log.LoadReservationsControllerPostScenario(log);

            var actual = sut.Post(dto);

            Assert.IsAssignableFrom<OkResult>(actual);
        }
    }
}
