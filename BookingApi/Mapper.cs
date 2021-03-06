﻿/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public static class Mapper
    {
        public static Reservation Map(ReservationDto dto)
        {
            return new Reservation
            {
                Id = dto.Id,
                Date = DateTime.Parse(dto.Date),
                Email = dto.Email,
                Name = dto.Name,
                Quantity = dto.Quantity
            };
        }
    }
}
