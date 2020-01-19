using System;
using System.Collections.Generic;
using System.Text;
using Funda.Ranker.Logging;
using Funda.Ranker.Models;
using Funda.Ranker.Services;
using SimpleInjector;

namespace Funda.Ranker.IoC
{
    public static class ContainerExtensions
    {
        public static void RegisterSimpleRanker(this Container container, int pageSize, int sleepTimeOnRequestLimitExceededInMilliSeconds, string baseUrl, int maxRetries)
        {
            container.Register<IObjectService, ObjectService>();
            container.RegisterBaseRanker(pageSize, sleepTimeOnRequestLimitExceededInMilliSeconds, baseUrl, maxRetries);
        }

        public static void RegisterParallelRanker(this Container container, int pageSize, int sleepTimeOnRequestLimitExceededInMilliSeconds, string baseUrl, int maxRetries)
        {
            container.Register<IObjectService, ParallelObjectService>();
            container.RegisterBaseRanker(pageSize, sleepTimeOnRequestLimitExceededInMilliSeconds, baseUrl, maxRetries);
        }

        private static void RegisterBaseRanker(this Container container, int pageSize,
            int sleepTimeOnRequestLimitExceededInMilliSeconds, string baseUrl, int maxRetries)
        {
            container.Register<IRanker<Realtor, int>, SimpleRealtorRanker>();
            container.Register<ILogger, ConsoleLogger>();
            container.Register(() => new FundaConfiguration() { BaseUrl = baseUrl, MaxRetries = maxRetries, SleepTimeAfterExceedingRequestLimit = sleepTimeOnRequestLimitExceededInMilliSeconds });
            container.Register(() => new ServiceConfiguration() { PageSize = pageSize });
        }
    }
}
