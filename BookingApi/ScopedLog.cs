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
        private readonly List<Interaction> interactions;

        public ILog Log { get; }

        public ScopedLog(ILog log)
        {
            Log = log;
            interactions = new List<Interaction>();
        }

        public void StartScope()
        {
        }

        public void Observe(Interaction interaction)
        {
            if (interaction.Time == null)
                interaction.Time = DateTimeOffset.Now.ToString("o");
            interactions.Add(interaction);
        }

        public void EndScope()
        {
            dynamic json = new { interactions };
            var s = JsonConvert.SerializeObject(json, serializerSettings);
            Log.Info(s);
        }
    }
}