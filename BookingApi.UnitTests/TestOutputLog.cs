/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class TestOutputLog : ILog
    {
        public TestOutputLog(ITestOutputHelper output)
        {
            Output = output;
        }

        public ITestOutputHelper Output { get; }

        public void Debug(string message)
        {
            Write(message);
        }

        public void Error(string message)
        {
            Write(message);
        }

        public void Info(string message)
        {
            Write(message);
        }

        public void Warning(string message)
        {
            Write(message);
        }

        private void Write(string message)
        {
            Output.WriteLine("{0}: {1}", DateTimeOffset.Now, message);
        }
    }
}
