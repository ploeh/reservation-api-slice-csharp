/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ConstantClock : IClock
    {
        private readonly DateTime dateTime;

        public ConstantClock(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public DateTime GetCurrentDateTime()
        {
            return dateTime;
        }
    }
}
