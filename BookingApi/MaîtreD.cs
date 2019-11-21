﻿/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class MaîtreD
    {
        public MaîtreD(IReadOnlyCollection<Table> tables)
        {
            Capacity = tables.Sum(t => t.Seats);
            Tables = tables;
        }

        public int Capacity { get; }
        public IReadOnlyCollection<Table> Tables { get; }

        public bool CanAccept(
            IEnumerable<Reservation> reservations,
            Reservation reservation)
        {
            var reservedSeats = reservations.Sum(r => r.Quantity);

            if (Capacity < reservedSeats + reservation.Quantity)
                return false;

            return true;
        }
    }
}
