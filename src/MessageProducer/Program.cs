﻿using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using MessageProducer.Models;
using Task = MessageProducer.Models.Task;

namespace MessageProducer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configFilePath = Path.Join(Environment.CurrentDirectory, "config.json");
            if (!File.Exists(configFilePath))
                Console.WriteLine(
                    "Create a `config.json` with a `ConnectionString` property, pointing to your Storage Account");
            var serializerSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var configString = File.ReadAllText(configFilePath);
            var config = JsonSerializer.Deserialize<Config>(configString, serializerSettings);
            var queueClient = new QueueClient(config.ConnectionString, "aaa-tasks");

            Parallel.For(1, 10000, i =>
            {
                var t = new Task("Azure Function message no {0}.", i);
                var message = JsonSerializer.Serialize(t, serializerSettings);
                queueClient.SendMessage(Base64Encode(message));
                Console.WriteLine($"Message {i} was sent to Azure Storage Queue");
            });
        }

        private static string Base64Encode(string message)
        {
            var b = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(b);
        }
    }
}