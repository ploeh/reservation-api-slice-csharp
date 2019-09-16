/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    [ApiController, Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly MaîtreD maîtreD;

        public ReservationsController(
            int capacity,
            IReservationsRepository repository)
        {
            Repository = repository;
            Capacity = capacity;
            maîtreD = new MaîtreD(Capacity);
        }

        public IReservationsRepository Repository { get; }
        public int Capacity { get; }

        public ActionResult Post(ReservationDto dto)
        {
            if (!DateTime.TryParse(dto.Date, out var _))
                return BadRequest($"Invalid date: {dto.Date}.");

            Reservation reservation = Mapper.Map(dto);

            if (reservation.Date < DateTime.Now)
                return BadRequest($"Invalid date: {reservation.Date}.");

            var reservations = Repository.ReadReservations(reservation.Date);
            bool accepted = maîtreD.CanAccept(reservations, reservation);
            if (!accepted)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Couldn't accept.");

            Repository.Create(reservation);
            return Ok();
        }
    }
}
