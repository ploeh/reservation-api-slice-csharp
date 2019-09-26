/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public static class Log
    {
        public static string LoadEmbeddedFile(string fileName)
        {
            using (var s = typeof(Log).Assembly.GetManifestResourceStream($"Ploeh.Samples.BookingApi.UnitTests.{fileName}"))
            using (var sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

        public static (ReservationsController, ReservationDto) LoadReservationsControllerPostScenario(string json)
        {
            dynamic logEntry = JsonConvert.DeserializeObject(json);

            ReservationDto dto =
                logEntry.entry.input.dto.ToObject<ReservationDto>();

            var interactions =
                logEntry.interactions.ToObject<IEnumerable<Interaction>>();

            var times = new List<DateTime>();
            var reads = new ConcurrentDictionary<DateTime, Queue<IEnumerable<Reservation>>>();

            foreach (Interaction interaction in interactions)
            {
                switch (interaction.Operation)
                {
                    case "GetCurrentDateTime":
                        times.Add(DateTime.Parse(interaction.Output.ToString()));
                        break;
                    case "ReadReservations":
                        DateTime input = ((dynamic)interaction.Input).date;
                        var output = ((JToken)interaction.Output).ToObject<IEnumerable<Reservation>>();
                        reads.GetOrAdd(input, _ => new Queue<IEnumerable<Reservation>>()).Enqueue(output);
                        break;
                    default:
                        break;
                }
            }

            // Imagine that seatingDuration and tables was logged as well
            var seatingDuration = TimeSpan.FromHours(2.5);
            var tables =
                new[] { new Table(2), new Table(4), new Table(6), new Table(8) };
            var repository = new ReplayReservationsRepository(reads);
            var clock = new ReplayClock(times);
            var controller = new ReservationsController(
                seatingDuration,
                tables,
                repository,
                clock);

            return (controller, dto);
        }
    }
}
