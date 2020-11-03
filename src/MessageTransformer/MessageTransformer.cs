using System.Text.Json;
using MessageTransformer.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MessageTransformer
{
    public static class MessageTransformer
    {
        [FunctionName("MessageTransformer")]
        [return: Queue("aaa-transformed", Connection = "KedaWebinarStorageAccount")]
        public static string Run(
            [QueueTrigger("aaa-tasks", Connection = "KedaWebinarStorageAccount")]
            string queueItem,
            ILogger log)
        {
            log.LogInformation("Transforming Message");

            var task = JsonSerializer.Deserialize<Task>(queueItem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (task != null)
            {
                var transformedMessage = task.TransformMessage();
                log.LogInformation("Message composed and transformed");
                return JsonSerializer.Serialize(new {Message = transformedMessage}, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            log.LogWarning("Cant transform an empty Message");
            return null;
        }
    }
}