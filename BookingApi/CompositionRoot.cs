/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class CompositionRoot : IControllerActivator
    {
        public CompositionRoot(
            TimeSpan seatingDuration,
            IReadOnlyCollection<Table> tables,
            string connectionString,
            FileInfo logFile)
        {
            SeatingDuration = seatingDuration;
            Tables = tables;
            ConnectionString = connectionString;
            LogFile = logFile;
        }

        public TimeSpan SeatingDuration { get; }
        public IReadOnlyCollection<Table> Tables { get; }
        public string ConnectionString { get; }
        public FileInfo LogFile { get; }

        public object Create(ControllerContext context)
        {
            var controllerType =
                context.ActionDescriptor.ControllerTypeInfo.AsType();

            if (controllerType == typeof(ReservationsController))
                return new ReservationsController(
                    SeatingDuration,
                    Tables,
                    new SqlReservationsRepository(ConnectionString),
                    new SystemClock(),
                    new FileLog(LogFile));

            throw new InvalidOperationException(
                $"Unknown controller type: {controllerType}.");
        }

        public void Release(ControllerContext context, object controller)
        {
        }
    }
}
