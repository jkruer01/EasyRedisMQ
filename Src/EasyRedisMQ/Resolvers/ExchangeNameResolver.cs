using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Resolvers
{
    public class ExchangeNameResolver : IExchangeNameResolver
    {
        public string GetExchangeName(object message)
        {
            var fullName = message.GetType().FullName;
            return GenerateExchangeName(fullName);
        }

        public string GetExchangeName<T>() where T : class
        {
            var fullName = typeof(T).FullName;
            return GenerateExchangeName(fullName);
        }

        private string GenerateExchangeName(string fullName)
        {
            return string.Format("Exchange.{0}", fullName);
        }
    }
}
