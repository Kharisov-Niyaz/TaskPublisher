using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace TaskPublisher
{
    class Program
    {
        public const string dbName = "taskDbPublisher.db";
        static string GetHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            const string exchangeName = "EXCHANGE2";

            ConnectionFactory factory = new ConnectionFactory();
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            factory.RequestedHeartbeat = TimeSpan.FromSeconds(5);
            factory.TopologyRecoveryEnabled = true;

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    factory.HostName = "localhost";
                    channel.ExchangeDeclare(
                        exchangeName,
                        ExchangeType.Topic,
                        true,
                        false,
                        null);

                    using (TaskDbContext db = new TaskDbContext())
                    {
                        List<Packet> packets = db.packets.Where(wr => !wr.sended).ToList();
                        if (packets.Count > 0)
                        {
                            Console.WriteLine("Sending...... press ENTER to exit");
                            Task task = Task.Run(() =>
                            {
                                foreach (Packet packet in packets)
                                {
                                    if (cancellationToken.IsCancellationRequested)
                                        break;
                                    packet.sended = true;
                                    packet.sendDate = DateTime.Now;
                                    packet.hash = GetHash($"{Directory.GetCurrentDirectory()}\\{Program.dbName}");
                                    if (db.SaveChanges() > 0)
                                    {
                                        byte[] byteMessage = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(packet));
                                        channel.BasicPublish(
                                            exchangeName,
                                            "testTopic",
                                            null,
                                            byteMessage);
                                        Console.WriteLine($"id:{packet.id} message:{packet.message} date:{packet.sendDate} hash:{packet.hash}");
                                        Thread.Sleep(1000);
                                    }
                                }
                            }, cancellationToken).
                            ContinueWith(pTask =>
                            {
                                if (pTask.IsFaulted)
                                    Console.WriteLine("Ошибка при отправке пакетов");
                                else if(!cancellationToken.IsCancellationRequested)
                                    Console.WriteLine("Отправка пакетов закончена");
                            });

                            if (Console.ReadKey().Key == ConsoleKey.Enter)
                            {
                                if (!task.IsCompleted)
                                {
                                    cancellationTokenSource.Cancel();
                                    Console.WriteLine("Отправка пакетов отменена");
                                }
                            }
                            cancellationTokenSource.Dispose();
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Отсутствуют пакеты для отправки");
                            Console.ReadLine();
                        }
                    }
                }
            }
        }

    }
}
