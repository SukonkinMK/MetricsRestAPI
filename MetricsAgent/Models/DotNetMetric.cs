﻿namespace MetricsAgent.Models
{
    public class DotNetMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public TimeSpan Time { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Value} - {Time}";
        }
    }
}
