// This is the default URL for triggering event grid function in the local environment.
// http://localhost:7071/admin/extensions/EventGridExtensionConfig?functionName={functionname} 

using bootCamp.Shared.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bootCamp.AzureFunctions
{
    public static class CreateBlobEvent
    {
        [FunctionName("CreateBlobEvent")]
        public static void Run([EventGridTrigger]JObject eventGridEvent, TraceWriter log)
        {
            var (action, fileName) = EventGridHelper.GetEventInfo(eventGridEvent);

            log.Info(eventGridEvent.ToString(Formatting.Indented));
            
        }
    }
}
