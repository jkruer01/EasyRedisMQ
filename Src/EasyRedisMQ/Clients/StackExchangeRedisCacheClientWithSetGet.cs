using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Clients
{
    public class StackExchangeRedisCacheClientWithSetGet : StackExchangeRedisCacheClient, ICacheClientExtended
    {
        public StackExchangeRedisCacheClientWithSetGet(ISerializer serializer) : this(serializer, null) { }
        public StackExchangeRedisCacheClientWithSetGet(ISerializer serializer, IRedisCachingConfiguration configuration = null) : base(serializer, configuration) { }
        public StackExchangeRedisCacheClientWithSetGet(ConnectionMultiplexer connectionMultiplexer, ISerializer serializer, int database = 0) : base(connectionMultiplexer, serializer, database) { }
        public StackExchangeRedisCacheClientWithSetGet(ISerializer serializer, string connectionString, int database = 0) : base(serializer, connectionString, database) { }

        public async Task<List<T>> SetMembersAsync<T>(string key)
        {
            var membersByteArray = await Database.SetMembersAsync(key);

            return membersByteArray.Select(m => Serializer.Deserialize<T>(m)).ToList();
        }
    }
}
