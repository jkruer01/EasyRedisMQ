using EasyRedisMQ.Clients;
using SharedModels;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Jil;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Threading.Tasks;

namespace EasyRedisMQ.Server
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

                _.Policies.ConstructorSelector<StackExchangeRedisCacheClientWithSetGetCtorRule>();
                _.For<ISerializer>().Singleton().Use<JilSerializer>().SelectConstructor(() => new JilSerializer());
                _.For<ICacheClientExtended>().Singleton()
                    .Use<StackExchangeRedisCacheClientWithSetGet>();
                _.For<IMessageBroker>().Singleton().Use<MessageBroker>();
            });

            var messageBroker = container.GetInstance<IMessageBroker>();
            
            var subscriber = messageBroker.SubscribeAsync<ConsoleMessage>("EasyRedisMQ.Server", async x => { await WriteConsoleMessageAsync(x); });



            Console.WriteLine("Listening for messages. Ctrl+C to quit.");
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
