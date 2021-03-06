/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Ploeh.Samples.BookingApi
{
    public class ScopedLog
    {
        private readonly static JsonSerializerSettings serializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        private Interaction entry;
        private readonly List<Interaction> interactions;

        public ILog Log { get; }

        public ScopedLog(ILog log)
        {
            Log = log;
            interactions = new List<Interaction>();
        }

        public void StartScope(Interaction entry)
        {
            if (entry.Time == null)
                entry.Time = DateTimeOffset.Now.ToString("o");
            this.entry = entry;
        }

        public void Observe(Interaction interaction)
        {
            if (interaction.Time == null)
                interaction.Time = DateTimeOffset.Now.ToString("o");
            interactions.Add(interaction);
        }

        public void EndScope(Interaction exit)
        {
            if (exit.Time == null)
                exit.Time = DateTimeOffset.Now.ToString("o");

            dynamic json = new { entry, interactions, exit };
            var s = JsonConvert.SerializeObject(json, serializerSettings);
            Log.Info(s);
        }
    }
}