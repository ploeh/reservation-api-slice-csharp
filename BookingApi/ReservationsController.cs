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
        public ReservationsController(
            IValidator validator,
            IMapper mapper,
            IMaîtreD maîtreD)
        {
            Validator = validator;
            Mapper = mapper;
            MaîtreD = maîtreD;
        }

        public IValidator Validator { get; }
        public IMapper Mapper { get; }
        public IMaîtreD MaîtreD { get; }

        public ActionResult Post(ReservationDto dto)
        {
            var validationMsg = Validator.Validate(dto);
            if (validationMsg != "")
                return BadRequest(validationMsg);

            var reservation = Mapper.Map(dto);
            var id = MaîtreD.TryAccept(reservation);
            if (id == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Couldn't accept.");
            return Ok(id.Value);
        }
    }
}
