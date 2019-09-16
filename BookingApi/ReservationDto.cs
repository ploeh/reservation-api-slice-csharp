/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;

namespace Ploeh.Samples.BookingApi
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}