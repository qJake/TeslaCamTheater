using System;

namespace TeslaCamTheater.Models
{
    public class Clipset
    {
        public string FrontCamera { get; set; }
        public string RearCamera { get; set; }
        public string LeftRepeaterCamera { get; set; }
        public string RightRepeaterCamera { get; set; }
        public DateTime StartTime { get; set; }
        public double DurationSeconds { get; set; }
        public bool HasEvent { get; set; }
        public double EventOffset { get; set; }
        public DateTime EndTime => StartTime.AddSeconds(DurationSeconds);

        public string StartTimeDisplay => StartTime.ToString("h:mm tt");

        public override string ToString()
        {
            return $"Clipset starting on {StartTime:MMM d} at {StartTime:hh:mm:ss tt} for {DurationSeconds} second(s)";
        }
    }
}
