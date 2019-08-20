using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public static class Resemblance
    {
        private readonly static ReservationComparer reservationComparer =
            new ReservationComparer();

        public static Expression<Func<Reservation, bool>> Like(Reservation x)
        {
            return y => reservationComparer.Equals(x, y);
        }
    }
}
