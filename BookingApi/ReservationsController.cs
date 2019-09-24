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
            IClock clock,
            ILog log)
        {
            SeatingDuration = seatingDuration;
            Tables = tables;
            Repository = repository;
            Clock = clock;
            Log = log;
            maîtreD = new MaîtreD(seatingDuration, tables);
        }

        public TimeSpan SeatingDuration { get; }
        public IReadOnlyCollection<Table> Tables { get; }
        public IReservationsRepository Repository { get; }
        public IClock Clock { get; }
        public ILog Log { get; }

        public ActionResult Post(ReservationDto dto)
        {
            Log.Debug($"Entering {nameof(Post)} method...");
            if (!DateTime.TryParse(dto.Date, out var _))
            {
                Log.Warning("Invalid reservation date.");
                return BadRequest($"Invalid date: {dto.Date}.");
            }

            Log.Debug("Mapping DTO to Domain Model.");
            Reservation reservation = Mapper.Map(dto);

            if (reservation.Date < Clock.GetCurrentDateTime())
            {
                Log.Warning("Invalid reservation date.");
                return BadRequest($"Invalid date: {reservation.Date}.");
            }

            Log.Debug("Reading existing reservations from database.");
            var reservations = Repository.ReadReservations(reservation.Date);
            bool accepted = maîtreD.CanAccept(reservations, reservation);
            if (!accepted)
            {
                Log.Warning("Not enough capacity");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Couldn't accept.");
            }

            Log.Info("Adding reservation to database.");
            Repository.Create(reservation);
            Log.Debug($"Leaving {nameof(Post)} method...");
            return Ok();
        }
    }
}
