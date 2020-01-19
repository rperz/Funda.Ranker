using System;

namespace Funda.Ranker.Logging
{
    public interface ILogger
    {
        void Error(string message, Exception exception);
        void Info(string message);
    }
}