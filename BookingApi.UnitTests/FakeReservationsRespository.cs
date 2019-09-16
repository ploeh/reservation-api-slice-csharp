/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class FakeReservationsRepository : 
        Dictionary<int, Reservation>, IReservationsRepository
    {
        private int nextKey;

        public void Create(Reservation reservation)
        {
            // Not thread-safe...
            var key = ++nextKey;
            Add(key, reservation);
        }

        public IEnumerable<Reservation> ReadReservations(DateTime date)
        {
            var min = date.Date;
            var max = date.Date.AddDays(1).AddTicks(-1);

            return Values
                .Where(r => min <= r.Date && r.Date <= max)
                .ToArray();
        }
    }
}
