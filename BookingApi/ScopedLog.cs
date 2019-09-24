/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ploeh.Samples.BookingApi
{
    public class ScopedLog
    {
        private readonly List<Interaction> observations;

        public ILog Log { get; }

        public ScopedLog(ILog log)
        {
            Log = log;
            observations = new List<Interaction>();
        }

        public void StartScope()
        {
        }

        public void Observe(Interaction interaction)
        {
            observations.Add(interaction);
        }

        public void EndScope()
        {
            dynamic json = new { observations };
            var s = JsonConvert.SerializeObject(json);
            Log.Info(s);
        }
    }
}