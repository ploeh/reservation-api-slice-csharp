/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class LogClock : IClock
    {
        public LogClock(IClock inner, ScopedLog log)
        {
            Inner = inner;
            Log = log;
        }

        public IClock Inner { get; }
        public ScopedLog Log { get; }

        public DateTime GetCurrentDateTime()
        {
            var currentDateTime = Inner.GetCurrentDateTime();
            Log.Observe(
                new Interaction
                {
                    Operation = nameof(GetCurrentDateTime),
                    Output = currentDateTime
                });
            return currentDateTime;
        }
    }
}
