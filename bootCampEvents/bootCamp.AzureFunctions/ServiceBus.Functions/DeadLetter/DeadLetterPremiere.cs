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

namespace bootCamp.AzureFunctions.ServiceBus.Functions.DeadLetter
{
    public static class DeadLetterPremiere
    {
        private static readonly TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "DeadLetterPremiere";

        [FunctionName(functionName)]
        public static void Run([ServiceBusTrigger(QueueNames.PremiereDeadLetter, AccessRights.Manage, Connection = "AzureWebJobsServiceBus")]string sbMessage, TraceWriter log)
        {
            LoggerHelper.WriteTrace(functionName, $"Atención se ha enviado a {QueueNames.PremiereDeadLetter} | {sbMessage} | a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);

            try
            {
                var hotelMessage = JsonConvert.DeserializeObject<Hotel>(sbMessage);
                var message = $@"
            Hola. {Environment.NewLine}
            No se ha podido procesar el fichero {hotelMessage.id} | {hotelMessage.name}. {Environment.NewLine}
            Un saludo.";
                var lista = "jorge.fernandez@encamina.com;jorgeffernandez@gmail.com";
                var listTo = lista.Split(';');
                EmailHelper.SendMail("Error en BootCampEvents", message, listTo);
                LoggerHelper.WriteTrace(functionName, $"Se ha enviado mail a {lista} avisando de la recepción errónea del fichero {hotelMessage.id} | {hotelMessage.name} a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteTrace(functionName, $"Se ha producido un error al enviar el mensaje: {ex.Message}, {ex.InnerException}", log, TraceLevel.Error, _telemetry);
            }
            LoggerHelper.WriteTrace(functionName, $"C# ServiceBus queue trigger function finished at {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
        }
    }
}
