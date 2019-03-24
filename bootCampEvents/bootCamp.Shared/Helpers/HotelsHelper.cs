using bootCamp.Shared.Constants;
using bootCamp.Shared.Entities;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace bootCamp.Shared.Helpers
{
    public class HotelsHelper
    {
        public async Task ProccessByFileAsync(string fileName, string functionName, TraceWriter log, TelemetryClient _telemetry)
        {
            LoggerHelper.WriteTrace(functionName, $"Recuperando el fichero {fileName} | a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);

            HotelsJson hotelList = new HotelsJson();
            List<Hotel> hoteles = new List<Hotel>();

            var serializer = new JsonSerializer();
            hotelList = JsonConvert.DeserializeObject<HotelsJson>(await BlobHelper.GetAsText(fileName, "demo"));
            hoteles = hotelList.Hotel.ToList();

            LoggerHelper.WriteTrace(functionName, $"Recuperando {hoteles.Count()} hoteles en la oferta | {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);

            List<Hotel> hotelesLowCost = new List<Hotel>();
            List<Hotel> hotelesPremiere = new List<Hotel>();
            List<Hotel> hotelesLuxury = new List<Hotel>();

            hotelesLowCost = hoteles.Where(o => o.minPrice <= 200).ToList();
            hotelesPremiere = hoteles.Where(o => o.minPrice > 200).ToList();
            hotelesLuxury = hoteles.Where(o => o.category == "5").ToList();

            AzureServiceBusHelper.CreateQueue(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], QueueNames.LowCost);

            foreach (var hotel in hotelesLowCost)
            {
                AzureServiceBusHelper.SendMessage(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], QueueNames.LowCost, JsonConvert.SerializeObject(hotel));
                LoggerHelper.WriteTrace(functionName, $"Detectado hotel {QueueNames.LowCost} | {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
            }

            AzureServiceBusHelper.CreateQueue(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], QueueNames.Premiere);

            foreach (var hotel in hotelesPremiere)
            {
                AzureServiceBusHelper.SendMessage(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], QueueNames.Premiere, JsonConvert.SerializeObject(hotel));
                LoggerHelper.WriteTrace(functionName, $"Detectado hotel {QueueNames.Premiere} | {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
            }

            AzureServiceBusHelper.CreateQueue(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], QueueNames.Luxury);

            foreach (var hotel in hotelesLuxury)
            {
                AzureServiceBusHelper.SendMessage(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], QueueNames.Luxury, JsonConvert.SerializeObject(hotel));
                LoggerHelper.WriteTrace(functionName, $"Detectado hotel {QueueNames.Luxury} | {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
            }

            LoggerHelper.WriteTrace(functionName, $"Se han procesado los {hoteles.Count()} hoteles | {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
        }
    }
}
