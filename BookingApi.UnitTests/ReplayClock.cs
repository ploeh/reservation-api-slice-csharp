/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReplayClock : IClock
    {
        private readonly Queue<DateTime> times;

        public ReplayClock(IEnumerable<DateTime> times)
        {
            this.times = new Queue<DateTime>(times);
        }

        public DateTime GetCurrentDateTime()
        {
            return times.Dequeue();
        }
    }
}
