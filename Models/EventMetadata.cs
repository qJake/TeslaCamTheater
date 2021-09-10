using Newtonsoft.Json;
using System;
using System.Linq;

namespace TeslaCamTheater.Models
{
    public class EventMetadata
    {
        public DateTime? Timestamp { get; set; }
        public string City { get; set; }

        [JsonProperty("est_lat")]
        public string Latitude { get; set; }

        [JsonProperty("est_lon")]
        public string Longitude { get; set; }

        public string Reason { get; set; }

        public string Camera { get; set; }

        [JsonIgnore]
        public string ReasonDescription => Constants.ReasonDescriptions[Constants.ReasonDescriptions.Keys.First(k => Reason.StartsWith(k))];

        public override string ToString()
        {
            return $"Event metadata on {Timestamp:MMM d} at {Timestamp:hh:mm:ss tt} in {City} via camera {Camera} due to {ReasonDescription}";
        }
    }
}

/*{
	"timestamp":"2021-09-09T18:50:00",
	"city":"Lombard",
	"est_lat":"41.8873",
	"est_lon":"-88.0181",
	"reason":"user_interaction_honk",
	"camera":"0"
}*/
