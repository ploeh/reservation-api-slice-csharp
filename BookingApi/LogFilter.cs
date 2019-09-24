/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class LogFilter : IActionFilter
    {
        public ConcurrentDictionary<object, ScopedLog> Logs { get; }

        public LogFilter(ConcurrentDictionary<object, ScopedLog> logs)
        {
            Logs = logs;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (Logs.TryGetValue(context.Controller, out var l))
                l.StartScope();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (Logs.TryRemove(context.Controller, out var l))
                l.EndScope();
        }
    }
}
