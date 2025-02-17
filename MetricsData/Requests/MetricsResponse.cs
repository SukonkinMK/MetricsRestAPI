 namespace MetricsData.Requests
{
    public class MetricsResponse<T> where T : class
    {
        public List<T> Metrics { get; set; }
        public int AgentId { get; set; }
    }
}
