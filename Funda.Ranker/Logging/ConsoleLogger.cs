using System;

namespace Funda.Ranker.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
