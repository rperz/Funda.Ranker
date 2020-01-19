using System;
using System.Collections.Generic;
using System.Text;
using Funda.Ranker.Services;
using SimpleInjector;

namespace Funda.Ranker.IoC
{
    public static class ContainerExtensions
    {
        public static void RegisterSimpleRanker(this Container container, int pageSize, int sleepTimeOnRequestLimitExceededInSeconds, string baseUrl, int maxRetries)
        {
            container.Register<IObjectService, ObjectService>();
            container.Register<IRanker<Realtor, int>, SimpleRealtorRanker>();
            container.Register<ILogger, ConsoleLogger>();
            container.Register(() => new FundaConfiguration() { BaseUrl = baseUrl, MaxRetries = maxRetries, SleepTimeAfterExceedingRequestLimit = sleepTimeOnRequestLimitExceededInSeconds});
            container.Register(()=>new ServiceConfiguration() { PageSize = pageSize });
        }

        public static void RegisterParallelRanker(this Container container, int pageSize, int sleepTimeOnRequestLimitExceededInSeconds, string baseUrl, int maxRetries)
        {
            container.Register<IObjectService, ParallelObjectService>();
            container.Register<IRanker<Realtor, int>, SimpleRealtorRanker>();
            container.Register<ILogger, ConsoleLogger>();
            container.Register(() => new FundaConfiguration() { BaseUrl = baseUrl, MaxRetries = maxRetries, SleepTimeAfterExceedingRequestLimit = sleepTimeOnRequestLimitExceededInSeconds });
            container.Register(() => new ServiceConfiguration() { PageSize = pageSize });
        }
    }
}
