// This is the default URL for triggering event grid function in the local environment.
// http://localhost:7071/admin/extensions/EventGridExtensionConfig?functionName={functionname} 

using bootCamp.Shared.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace bootCamp.AzureFunctions
{
    public static class CreateBlobEvent
    {
        private static TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "CreateBlobEvent";

        [FunctionName(functionName)]
        public static async Task Run([EventGridTrigger]JObject eventGridEvent, TraceWriter log)
        {
            var (action, fileName) = EventGridHelper.GetEventInfo(eventGridEvent);

            try
            {

                LoggerHelper.WriteTrace(functionName, $"Procesando el blob {fileName} | a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);

                if (action.Equals("Microsoft.Storage.BlobCreated"))
                {
                    await new HotelsHelper().ProccessByFileAsync(fileName, functionName, log, _telemetry);
                }
                else
                {
                    LoggerHelper.WriteTrace(functionName, $"Evento {action} | a verlas pasar {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteTrace(functionName, $"Se ha producido un error en {fileName} | {action} a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}. Error: {ex} , {ex.InnerException} ", log, TraceLevel.Error, _telemetry);
                throw;
            }

            log.Info(eventGridEvent.ToString(Formatting.Indented));
            }
        }
    }
