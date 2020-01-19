namespace Funda.Ranker
{
    public class FundaConfiguration
    {
        public string BaseUrl { get; set; }
        public int MaxRetries { get; set; }
        public int SleepTimeAfterExceedingRequestLimit { get; set; }
    }
}