using EasyRedisMQ.Clients;
using SharedModels;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Jil;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Diagnostics;

namespace EasyRedisMQ.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.Assembly("EasyRedisMQ");
                    x.WithDefaultConventions();
                });
                
                _.Policies.ConstructorSelector<StackExchangeRedisCacheClientWithSetGetCtorRule>();
                _.For<ISerializer>().Singleton().Use<JilSerializer>().SelectConstructor(() => new JilSerializer());
                _.For<ICacheClientExtended>().Singleton()
                    .Use<StackExchangeRedisCacheClientWithSetGet>();
                _.For<IMessageBroker>().Singleton().Use<MessageBroker>();
            });


            var messageBroker = container.GetInstance<IMessageBroker>();

            var stopWatch = new Stopwatch();
            int numberOfMessagesToPublish = 100000;
            stopWatch.Start();
            for(int x = 0; x <= numberOfMessagesToPublish; x++)
            {
                messageBroker.PublishAsync<ConsoleMessage>(new ConsoleMessage
                {
                    Message = x.ToString()
                });
            }
            stopWatch.Stop();
            System.Console.WriteLine("Published {0} messages in {1} seconds. {2} messages per second.", numberOfMessagesToPublish, stopWatch.Elapsed.TotalSeconds, numberOfMessagesToPublish / stopWatch.Elapsed.TotalSeconds);
            
            while(true)
            {
                System.Console.WriteLine("Enter message to be published. Ctrl+C to quit.");
                var message = System.Console.ReadLine();
                messageBroker.PublishAsync<ConsoleMessage>(new ConsoleMessage
                {
                    Message = message
                });
                System.Console.WriteLine("Message published successfully.");
            }


        }
    }
}
