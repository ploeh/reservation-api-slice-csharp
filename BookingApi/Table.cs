using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class Table
    {
        public Table(int seats)
        {
            Seats = seats;
        }

        public int Seats { get; }

        public override bool Equals(object obj)
        {
            return obj is Table table &&
                   Seats == table.Seats;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Seats);
        }
    }
}
