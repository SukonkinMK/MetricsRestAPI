namespace MetricsAgent.Models
{
    public class NetworkMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }

        public void Update(NetworkMetric metric)
        {
            Value = metric.Value;
            Time = metric.Time;
        }

        public override string ToString()
        {
            return $"{Id} - {Value} - {Time}";
        }
    }
}
