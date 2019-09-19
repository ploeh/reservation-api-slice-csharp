/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class FileLog : ILog
    {
        public FileLog(FileInfo logFile)
        {
            LogFile = logFile;
        }

        public FileInfo LogFile { get; }

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
            using (var writer = LogFile.AppendText())
                writer.WriteLine("{0}: {1}", DateTimeOffset.Now, message);
        }
    }
}
