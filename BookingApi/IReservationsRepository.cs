﻿/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;

namespace Ploeh.Samples.BookingApi
{
    public interface IReservationsRepository
    {
        IEnumerable<Reservation> ReadReservations(DateTime date);
        void Create(Reservation reservation);
    }
}