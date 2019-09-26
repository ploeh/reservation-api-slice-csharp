/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReplayReservationsRepository : IReservationsRepository
    {
        private readonly IDictionary<DateTime, Queue<IEnumerable<Reservation>>> reads;

        public ReplayReservationsRepository(
            IDictionary<DateTime, Queue<IEnumerable<Reservation>>> reads)
        {
            this.reads = reads;
        }

        public void Create(Reservation reservation)
        {
        }

        public IEnumerable<Reservation> ReadReservations(DateTime date)
        {
            return reads[date].Dequeue();
        }
    }
}
