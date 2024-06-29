namespace MetricsAgent.Models
{
    public class RamMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }

        public void Update(RamMetric metric)
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
