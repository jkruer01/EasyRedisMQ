using EasyRedisMQ.Clients;
using SharedModels;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Jil;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Diagnostics;

namespace EasyRedisMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sleep for 2 seconds to give the consumers time to start and subscribe to the message exchange.
            System.Threading.Thread.Sleep(2000);

            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.Assembly("EasyRedisMQ");
                    x.WithDefaultConventions();
                });

                _.For<ISerializer>().Singleton().Use(c => new JilSerializer());
                _.For<ICacheClientExtended>().Singleton().Use(c => new StackExchangeRedisCacheClient(c.GetInstance<ISerializer>(), null));
                _.For<IMessageBroker>().Singleton().Use<MessageBroker>();
            });


            var messageBroker = container.GetInstance<IMessageBroker>();

            var stopWatch = new Stopwatch();
            int numberOfMessagesToPublish = 10000;
            stopWatch.Start();
            for(int x = 0; x <= numberOfMessagesToPublish; x++)
            {
                messageBroker.PublishAsync<ConsoleMessage>(new ConsoleMessage
                {
                    Message = x.ToString()
                });
            }
            stopWatch.Stop();
            Console.WriteLine("Published {0} messages in {1} seconds. {2} messages per second.", numberOfMessagesToPublish, stopWatch.Elapsed.TotalSeconds, numberOfMessagesToPublish / stopWatch.Elapsed.TotalSeconds);
            
            while(true)
            {
                Console.WriteLine("Enter message to be published. Ctrl+C to quit.");
                var message = Console.ReadLine();
                messageBroker.PublishAsync<ConsoleMessage>(new ConsoleMessage
                {
                    Message = message
                });
                Console.WriteLine("Message published successfully.");
            }
        }
    }
}
