/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
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
        public ReservationsController(IValidator validator)
        {
            Validator = validator;
        }

        public IValidator Validator { get; }

        public ActionResult Post(ReservationDto dto)
        {
            return BadRequest(Validator.Validate(dto));
        }
    }
}
