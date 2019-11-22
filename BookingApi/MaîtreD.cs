/* Copyright (c) Mark Seemann 2019 all rights reserved
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
        public MaîtreD(
            TimeSpan seatingDuration,
            IReadOnlyCollection<Table> tables) :
            this(seatingDuration, tables.ToArray())
        {
        }

        public MaîtreD(TimeSpan seatingDuration, params Table[] tables)
        {
            Tables = tables.OrderBy(t => t.Seats).ToArray();
            SeatingDuration = seatingDuration;
        }

        public TimeSpan SeatingDuration { get; }
        public IReadOnlyCollection<Table> Tables { get; }

        public bool CanAccept(
            IEnumerable<Reservation> reservations,
            Reservation reservation)
        {
            var remainingTables = Tables.ToList();
            foreach (var r in reservations.OrderBy(x => x.Quantity))
            {
                var idx =
                    remainingTables.FindIndex(t => r.Quantity <= t.Seats);
                if (idx < 0)
                    return false;
                remainingTables.RemoveAt(idx);
            }
            return remainingTables.Any(t => reservation.Quantity <= t.Seats);
        }
    }
}
