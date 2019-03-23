using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace bootCamp.Shared.Helpers
{
    public static class AzureServiceBusHelper
    {
        public static void CreateQueue(string connectionString, string queueName)
        {
            var manager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!manager.QueueExists(queueName))
            {
                manager.CreateQueue(new QueueDescription(queueName)
                {
                    Path = queueName
                });
            }
        }

        public static async Task CreateQueueAsync(string connectionString, string queueName)
        {
            var manager = NamespaceManager.CreateFromConnectionString(connectionString);
            var existsQueue = await manager.QueueExistsAsync(queueName).ConfigureAwait(false);

            if (!existsQueue)
            {
                await manager.CreateQueueAsync(new QueueDescription(queueName)
                {
                    Path = queueName
                }).ConfigureAwait(false);
            }
        }

        public static void SendMessage(string connectionString, string queueName, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var queueClient = InitializeQueueClient(connectionString, queueName);
                queueClient.Send(new BrokeredMessage(message));
            }
        }

        public static async Task SendMessageAsync(string connectionString, string queueName, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var queueClient = InitializeQueueClient(connectionString, queueName);
                await queueClient.SendAsync(new BrokeredMessage(message)).ConfigureAwait(false);
            }
        }

        private static QueueClient InitializeQueueClient(string connectionString, string queueName)
        {
            return QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

    }
}
