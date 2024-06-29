namespace MetricsAgent.Models
{
    public class DotNetMetricDto
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
        public int Id { get; set; }
    }
}
