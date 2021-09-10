using System.Collections.Generic;

namespace TeslaCamTheater.Models
{
    public static class Constants
    {
        public static readonly Dictionary<string, string> ReasonDescriptions = new Dictionary<string, string>
        {
            ["sentry_aware_object_detection"] = "Sentry Mode (Object)",
            ["sentry_aware_accel_"] = "Sentry Mode (Accelerometer)",
            ["sentry_"] = "General Sentry",
            ["user_interaction_dashcam_icon_tapped"] = "Saved (via Dashcam icon)",
            ["user_interaction_dashcam_panel_save"] = "Saved (via Dashcam panel)",
            ["user_interaction_honk"] = "Horn Honked",
            ["user_interaction_"] = "Genearl User Interaction"
        };

        // Not correct and not done yet - somehow there are 6-7 IDs...
        public static readonly Dictionary<string, string> CameraNames = new Dictionary<string, string>
        {
            ["0"] = "Front",
            ["3"] = "Left",
            ["4"] = "Right",
            ["5"] = "Rear"
        };

        // Found this online... might be accurate.
        //0 = front camera
        //1 = fisheye
        //2 = narrow
        //3 = left repeater
        //4 = right repeater
        //5 = left B pillar
        //6 = right B pillar
        //7 = rear
        //8 = cabin
    }
}
