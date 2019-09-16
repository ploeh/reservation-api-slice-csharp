/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
namespace Ploeh.Samples.BookingApi
{
    public class NullLog : ILog
    {
        public readonly static NullLog Singleton = new NullLog();

        private NullLog()
        {
        }

        public void Debug(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Info(string message)
        {
        }

        public void Warning(string message)
        {
        }
    }
}