using bootCamp.Shared.Constants;
using bootCamp.Shared.Entities;
using bootCamp.Shared.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace bootCamp.AzureFunctions.ServiceBus.Functions
{
    public static class GetLuxury
    {
        private static TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "GetLuxury";

        [FunctionName(functionName)]
        public static void Run([ServiceBusTrigger(QueueNames.Luxury, AccessRights.Manage, Connection = "AzureWebJobsServiceBus")]string sbMessage, TraceWriter log)
        {
            try
            {
                var hotel = JsonConvert.DeserializeObject<Hotel>(sbMessage);
                LoggerHelper.WriteTrace(functionName, $"{QueueNames.Luxury.ToUpper()} --> {hotel.name} con precio {hotel.minPrice}€ |a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);

            }
            catch (Exception e)
            {
                LoggerHelper.TraceException(functionName, log, _telemetry, e);
                throw e;
            }
        }
    }
}
