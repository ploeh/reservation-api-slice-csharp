/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class LogReservationsRepository : IReservationsRepository
    {
        public LogReservationsRepository(
            IReservationsRepository inner,
            ScopedLog log)
        {
            Inner = inner;
            Log = log;
        }

        public IReservationsRepository Inner { get; }
        public ScopedLog Log { get; }

        public void Create(Reservation reservation)
        {
            Log.Observe(
                new Interaction
                {
                    Operation = nameof(Create),
                    Input = reservation
                });
            Inner.Create(reservation);
        }

        public IEnumerable<Reservation> ReadReservations(DateTime date)
        {
            var reservations = Inner.ReadReservations(date);
            Log.Observe(
                new Interaction
                {
                    Operation = nameof(ReadReservations),
                    Input = date,
                    Output = reservations
                });
            return reservations;
        }
    }
}
