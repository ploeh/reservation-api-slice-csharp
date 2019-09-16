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
            IReservationsRepository repository,
            int capacity)
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

            var reservation = Mapper.Map(dto);
            var reservations = Repository.ReadReservations(reservation.Date);

            var accepted = maîtreD.CanAccept(reservations, reservation);
            if (!accepted)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Couldn't accept.");

            Repository.Create(reservation);
            return Ok();
        }
    }
}
