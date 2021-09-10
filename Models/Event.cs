using System.Collections.Generic;

namespace TeslaCamTheater.Models
{
    /// <summary>
    /// Represents an individual camera event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Gets or sets the event metadata for this event.
        /// </summary>
        public EventMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the clips in this event.
        /// </summary>
        public List<Clipset> Clips { get; set; }

        /// <summary>
        /// Gets or sets the event type (Sentry, Saved, etc).
        /// </summary>
        public EventType Type { get; set; }

        public override string ToString()
        {
            return $"{Type} Event - {Clips.Count} clip(s)";
        }
    }
}
