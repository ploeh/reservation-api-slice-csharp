using System;
using System.Collections.Generic;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationComparer : IEqualityComparer<Reservation>
    {
        public bool Equals(Reservation x, Reservation y)
        {
            return Equals(x.Date, y.Date)
                && Equals(x.Email, y.Email)
                && Equals(x.Name, y.Name)
                && Equals(x.Quantity, y.Quantity);
        }

        public int GetHashCode(Reservation obj)
        {
            return
                obj.Date.GetHashCode() ^
                obj.Email.GetHashCode() ^
                obj.Name.GetHashCode() ^
                obj.Quantity.GetHashCode();
        }
    }
}
