namespace MetricsAgent.Models
{
    public class DotNetMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }

        public void Update(DotNetMetric metric)
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
