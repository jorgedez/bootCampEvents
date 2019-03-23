using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace bootCamp.Shared.Helpers
{
    public static class LoggerHelper
    {
        /// <summary>
        /// Write traces for exceptions
        /// </summary>
        /// <param name="customEventName"></param>
        /// <param name="log"></param>
        /// <param name="telemetry"></param>
        /// <param name="e"></param>
        public static void TraceException(string customEventName, TraceWriter log, TelemetryClient telemetry, Exception e)
        {
            if (e is DocumentClientException)
            {
                TraceCosmosDBError(customEventName, log, telemetry, e as DocumentClientException);
            }
            else
            {
                WriteTrace(customEventName, e.InnerException != null ? e.InnerException.Message : e.Message, log, TraceLevel.Error, telemetry, e);
            }
        }

        /// <summary>
        /// Write traces in webjob log and application insights
        /// </summary>
        /// <param name="customEventName"></param>
        /// <param name="message"></param>
        /// <param name="log"></param>
        /// <param name="traceLevel"></param>
        /// <param name="telemetry"></param>
        /// <param name="e"></param>
        public static void WriteTrace(string customEventName, string message, TraceWriter log, TraceLevel traceLevel, TelemetryClient telemetry, Exception e = null)
        {
            var traceActions = new Dictionary<TraceLevel, Action>
            {
                {
                    TraceLevel.Verbose, () => {
                        log.Verbose(message);
                        telemetry.TrackEvent(customEventName, new Dictionary<string, string>
                        {
                            { "Message", message },
                            { "SeverityLevel",  SeverityLevel.Verbose.ToString() }
                        });
                    }
                },
                {
                    TraceLevel.Info, () => {
                        log.Info(message);
                        telemetry.TrackEvent(customEventName, new Dictionary<string, string>
                        {
                            { "Message", message },
                            { "SeverityLevel",  SeverityLevel.Information.ToString() }
                        });
                    }
                },
                {
                    TraceLevel.Warning, () => {
                        log.Warning(message);
                        telemetry.TrackEvent(customEventName, new Dictionary<string, string>
                        {
                            { "Message", message },
                            { "SeverityLevel",  SeverityLevel.Warning.ToString() }
                        });
                    }
                },
                {
                    TraceLevel.Error, () => {
                        log.Error(message);

                        if(e != null)
                        {
                            telemetry.TrackException(e, new Dictionary<string, string>{
                                { "ErrorMessage", message}
                            });
                        }

                        telemetry.TrackEvent(customEventName, new Dictionary<string, string>
                        {
                            { "Message", message },
                            { "SeverityLevel",  SeverityLevel.Error.ToString() }
                        });
                    }
                }
            };

            traceActions[traceLevel]();
        }

        /// <summary>
        /// Write traces for IndexBatchException
        /// </summary>
        /// <param name="customEventName"></param>
        /// <param name="log"></param>
        /// <param name="telemetry"></param>
        /// <param name="e"></param>
        private static void TraceCosmosDBError(string customEventName, TraceWriter log, TelemetryClient telemetry, DocumentClientException e)
        {

            WriteTrace(customEventName, $@"Failed to upsert the document: ActivityId{e.ActivityId}:
                    - StatusCode: {e.StatusCode} - ErrorMessage: {e.Message}", log, TraceLevel.Error, telemetry, e);
        }
    }
}
