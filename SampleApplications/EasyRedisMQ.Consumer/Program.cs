using SharedModels;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Jil;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Threading.Tasks;

namespace EasyRedisMQ.Consumer
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

            //This uses the same Subscriber ID as EasyRedisMQ.Consumer.
            //These two applications will share the same queue and messages will be divided between the 2.
            var subscriber = messageBroker.SubscribeAsync<ConsoleMessage>("EasyRedisMQ.Consumer", async x => { await WriteConsoleMessageAsync(x); });
            
            Console.WriteLine("EasyRedisMQ.Consumer is listening for messages. Ctrl+C to quit.");
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
