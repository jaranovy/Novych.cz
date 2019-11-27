using Novych.Models.Common;
using Novych.Models.Database;
using System;

namespace Novych.Models
{

    public enum LogClass { DEBUG, INFO, WARNING, ERROR };

    public class Logger
    {
        private static NovychDbContext DB = new NovychDbContext();

        public static void log(LogClass logClass, string message)
        {
            Log logEntry = new Log();
            logEntry.Date = DateTime.Now;
            logEntry.Class = logClass.ToString();
            logEntry.Message = message;

#if DEBUG
            System.Diagnostics.Debug.WriteLine("[" + logEntry.Date.ToString("HH:mm:ss.fff") + "][" + logEntry.Class + "] " + logEntry.Message);
#endif

            if (UnitTestDetector.IsRunningFromNUnit == false)
            {
                DB.Logs.Add(logEntry);
                DB.SaveChanges();
            }
        }

        public static void logVisitor(string ipAddress, string identifier)
        {
            Visitor visitor = new Visitor();
            visitor.Date = DateTime.Now;
            visitor.IpAddress = ipAddress;
            visitor.Identifier = identifier;

#if DEBUG
            log(LogClass.INFO, "Log visitor [" + visitor.IpAddress + "] [" + visitor.Identifier + "]");
#endif

            if (UnitTestDetector.IsRunningFromNUnit == false)
            {
                DB.Visitors.Add(visitor);
                DB.SaveChanges();
            }
        }
    }
}