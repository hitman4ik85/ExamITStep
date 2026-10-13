using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using QueueExam.Models;
using Serilog;
using Serilog.Sinks.AzureBlobStorage;
using System.Text.Json;

namespace QueueExam
{
    public class Program
    {
        static QueueClient queueClient;
        static List<string> users = new List<string> { "Anna", "Petro", "Ivan", "Olena", "Marta" };

        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("config.json").Build();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.AzureBlobStorage(
                    connectionString: configuration["BlobConnString"],
                    storageContainerName: configuration["BlobContainer"],
                    storageFileName: "log.txt",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                )
                .CreateLogger();

            var queueName = configuration["QueueName"];
            var connectionString = configuration["AzureConnString"];
            queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Додати повiдомлення");
                Console.WriteLine("2. Прочитати повiдомлення");
                Console.WriteLine("3. Вийти");
                Console.Write("Оберiть опцiю: ");
                string option = Console.ReadLine();

                if (option == "1")
                    await AddMessages();
                else if (option == "2")
                    await ReadMessages();
                else if (option == "3")
                    break;
                else
                    Console.WriteLine("Невiрна опцiя!");
            }
        }

        private static async Task AddMessages()
        {
            List<Message> messages = new List<Message>
            {
                new Message { Content = "Meeting at 9AM", Receivers = "Anna" },
                new Message { Content = "Project deadline", Receivers = "Petro,Anna" },
                new Message { Content = "Team lunch today", Receivers = "Olena" },
                new Message { Content = "Client feedback", Receivers = "Ivan,Marta" },
                new Message { Content = "HR update", Receivers = "Petro" },
                new Message { Content = "Server reboot", Receivers = "Ivan,Olena,Anna" },
                new Message { Content = "Budget approved", Receivers = "Marta" },
                new Message { Content = "Company event", Receivers = "Petro,Olena" },
                new Message { Content = "Monthly report", Receivers = "Anna" },
                new Message { Content = "New intern", Receivers = "Ivan" }
            };

            foreach (var msg in messages)
            {
                string json = JsonSerializer.Serialize(msg);
                await queueClient.SendMessageAsync(json, null, TimeSpan.FromHours(24));
                Console.WriteLine($"Додано повiдомлення: {msg.Content} → {msg.Receivers}");
                Log.Information("Sent: {Content} → {Receivers}", msg.Content, msg.Receivers);
            }
        }

        private static async Task ReadMessages()
        {
            Console.WriteLine("\nКористувачи: " + string.Join(", ", users));
            Console.Write("Введiть своє iм'я: ");
            string name = Console.ReadLine();

            if (!users.Contains(name))
            {
                Console.WriteLine("⛔ Такого користувача не iснує!");
                return;
            }

            QueueMessage[] msgs = await queueClient.ReceiveMessagesAsync(32);
            foreach (var msg in msgs)
            {
                var m = JsonSerializer.Deserialize<Message>(msg.Body.ToString());
                List<string> receivers = m.Receivers.Split(',').Select(r => r.Trim()).ToList();
                List<string> readBy = m.ReadBy.Split(',').Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim()).ToList();

                if (receivers.Contains(name) && !readBy.Contains(name))
                {
                    readBy.Add(name);
                    m.ReadBy = string.Join(",", readBy);

                    Console.WriteLine($"\n📩 {m.Content} (для: {m.Receivers})");
                    Log.Information("{Name} прочитав: {Content}", name, m.Content);

                    if (readBy.Count == receivers.Count)
                    {
                        await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
                        Console.WriteLine("✅ Повiдомлення прочитали всi, воно видалене!");
                    }
                    else
                    {
                        string updated = JsonSerializer.Serialize(m);
                        await queueClient.SendMessageAsync(updated, null, TimeSpan.FromHours(24));
                        await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
                        Console.WriteLine("ℹ️ Повiдомлення прочитано, ще не всiма!");
                    }
                }
                else
                {
                    await queueClient.SendMessageAsync(msg.Body.ToString(), null, TimeSpan.FromHours(24));
                    await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
                }
            }
        }
    }
}
