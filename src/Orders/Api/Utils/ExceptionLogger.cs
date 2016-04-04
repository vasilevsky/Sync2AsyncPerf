using System;
using System.Web.Http.ExceptionHandling;

namespace API.Utils
{
    /// <summary>
    /// Represents Event Log logger of unhandled exceptions.
    /// </summary>
    public class EventLogExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Console.WriteLine(context.Exception.Message);
        }
    }
}