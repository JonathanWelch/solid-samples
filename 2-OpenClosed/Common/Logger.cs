using System;
using System.Collections.Generic;
using log4net;

namespace _2_OpenClosed.Common
{
    public class Logger
    {
        private static readonly List<ILog> Loggers;

        static Logger()
        {
            Loggers = new List<ILog>();
        }

        public static void Add(ILog log)
        {
            Loggers.Add(log);
        }

        public static void Clear()
        {
            Loggers.Clear();
        }

        public static void Debug(string message)
        {
            Loggers.ForEach(x => x.Debug(message));
        }

        public static void Debug(string message, Exception ex)
        {
            Loggers.ForEach(x => x.Debug(message, ex));
        }

        public static void Info(string message)
        {
            Loggers.ForEach(x => x.Info(message));
        }

        public static void Info(string message, Exception ex)
        {
            Loggers.ForEach(x => x.Info(message, ex));
        }

        public static void Warn(string message)
        {
            Loggers.ForEach(x => x.Warn(message));
        }

        public static void Warn(string message, Exception ex)
        {
            Loggers.ForEach(x => x.Warn(message, ex));
        }

        public static void Error(Exception exception)
        {
            Loggers.ForEach(x => x.Error(exception.Message, exception));
        }

        public static void Error(string message, Exception ex)
        {
            Loggers.ForEach(x => x.Error(message, ex));
        }
    }
}
