using EasyRedisMQ.Clients;
using StackExchange.Redis.Extensions.Core;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class StackExchangeRedisCacheClientWithSetGetCtorRule : IConstructorSelector
    {
        public ConstructorInfo Find(Type pluggedType, DependencyCollection dependencies, PluginGraph graph)
        {
            // if this rule does not apply to the pluggedType,
            // just return null to denote "not applicable"
            if (!pluggedType.CanBeCastTo<StackExchangeRedisCacheClientWithSetGet>()) return null;

            return pluggedType.GetConstructor(new[] { typeof(ISerializer) });
        }
    }
}
