using System;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MessageTransformer
{
    public static class MessageTransformer
    {
        [FunctionName("MessageTransformer")]
        [return: Queue("aaa-transformed", Connection = "KedaWebinarStorageAccount")]
        public static string Run(
            [QueueTrigger("aaa-tasks", Connection = "KedaWebinarStorageAccount")] string queueItem,
            ILogger log)
        {
            log.LogInformation("Transforming Message");

            var task = JsonSerializer.Deserialize<Models.Task>(queueItem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (task != null)
            {
                var transformedMessage = task.TransformMessage();
                log.LogInformation("Message composed and transformed");
                return JsonSerializer.Serialize(new { Message = transformedMessage }, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            }
            log.LogWarning("Cant transform an empty Message");
            return null;
        }
    }
}
