using System;
using System.IO;
using System.Text.Json;
using MessageDispatcher.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MessageDispatcher
{
    public static class MessageDispatcher
    {
        [FunctionName("MessageDispatcher")]
        public static void Run(
            [QueueTrigger("aaa-transformed", Connection = "KedaWebinarStorageAccount")]
            string queueItem,
            [Blob("processed/{rand-guid}.json", FileAccess.Write, Connection = "KedaWebinarStorageAccount")]
            Stream processedBlob,
            ILogger log)
        {
            log.LogInformation("Processing transformed Message");

            var task = JsonSerializer.Deserialize<TransformedMessage>(queueItem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (task != null)
            {
                var numberOfKittens = task.ExtractNumberOfKittens();
                using (var writer = new StreamWriter(processedBlob))
                {
                    writer.Write(JsonSerializer.Serialize(new
                    {
                        Id = Guid.NewGuid(),
                        NumberOfKittens = numberOfKittens,
                        AreaRequirements = $"{numberOfKittens * 0.75}qm"
                    }, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
                    log.LogInformation("Message destructed and processed");
                }

                return;
            }

            log.LogWarning("Cant process am empty message");
        }
    }
}