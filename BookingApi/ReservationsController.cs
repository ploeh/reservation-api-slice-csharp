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
            TimeSpan seatingDuration,
            IReadOnlyCollection<Table> tables,
            IReservationsRepository repository,
            ILog log)
        {
            SeatingDuration = seatingDuration;
            Tables = tables;
            Repository = repository;
            Log = log;
            maîtreD = new MaîtreD(seatingDuration, tables);
        }

        public TimeSpan SeatingDuration { get; }
        public IReadOnlyCollection<Table> Tables { get; }
        public IReservationsRepository Repository { get; }
        public ILog Log { get; }

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
