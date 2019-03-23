using Newtonsoft.Json.Linq;

namespace bootCamp.Shared.Helpers
{
    public static class EventGridHelper
    {
        public static (string action, string fileName) GetEventInfo(JObject eventGridEvent)
        {
            return (action: (string)eventGridEvent["eventType"],
                fileName: System.IO.Path.GetFileName((string)eventGridEvent["data"]["url"]));
        }
    }
}
