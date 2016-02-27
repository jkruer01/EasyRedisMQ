using SharedModels;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Jil;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Threading.Tasks;

namespace EasyRedisMQ.ADifferentConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.Assembly("EasyRedisMQ");
                    x.WithDefaultConventions();
                });

                _.For<ISerializer>().Singleton().Use(c => new JilSerializer());
                _.For<ICacheClient>().Singleton().Use(c => new StackExchangeRedisCacheClient(c.GetInstance<ISerializer>(), null));
            });
            
            var messageBroker = container.GetInstance<IMessageBroker>();

            //The subscriber ID for this application is different from EasyRedisMQ.Consumer or EasyRedisMQ.Consumer2.
            //This consumer will get its own queue and it gets its own copy of every message published.
            var subscriber = messageBroker.SubscribeAsync<ConsoleMessage>("EasyRedisMQ.ADifferentConsumer", async x => { await WriteConsoleMessageAsync(x); });
            
            Console.WriteLine("EasyRedisMQ.ADifferentConsumer is listening for messages. Ctrl+C to quit.");
            while(true)
            {
                System.Threading.Thread.Sleep(500);
            }
        }

        public static Task WriteConsoleMessageAsync(ConsoleMessage message)
        {
            Console.WriteLine(message.Message);
            return Task.FromResult(0);
        }
    }
}
