/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class CompositionRoot : IControllerActivator
    {
        public CompositionRoot(int capacity, string connectionString)
        {
            Capacity = capacity;
            ConnectionString = connectionString;
        }

        public int Capacity { get; }
        public string ConnectionString { get; }

        public object Create(ControllerContext context)
        {
            var controllerType =
                context.ActionDescriptor.ControllerTypeInfo.AsType();

            if (controllerType == typeof(ReservationsController))
                return new ReservationsController(
                    new Validator(),
                    new Mapper(),
                    new MaîtreD(
                        Capacity, 
                        new SqlReservationsRepository(ConnectionString)));

            throw new InvalidOperationException(
                $"Unknown controller type: {controllerType}.");
        }

        public void Release(ControllerContext context, object controller)
        {
        }
    }
}
