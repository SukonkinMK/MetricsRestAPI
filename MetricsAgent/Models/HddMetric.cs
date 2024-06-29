namespace MetricsAgent.Models
{
    public class HddMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }

        public void Update(HddMetric metric)
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
