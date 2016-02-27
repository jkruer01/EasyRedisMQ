using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyRedisMQ.Resolvers;

namespace EasyRedisMQ.Tests.Resolvers
{
    [TestClass]
    public class QueueNameResolverTests
    {
        private QueueNameResolver _queueNameResolver;
        private readonly string _exchangeName = "exchangeName";
        private readonly string _subscriberId = "subscriberId";

        [TestInitialize]
        public void InitializeAll_QueueNameResolverTests()
        {
            _queueNameResolver = new QueueNameResolver();
        }

        [TestMethod]
        public void QueueNameResolver_GetQueueName_ShouldReturn_ExchangeNamePlusQueuePlusSubscriberId()
        {
            var expectedResult = string.Format("{0}.Queue.{1}", _exchangeName, _subscriberId);

            var result = _queueNameResolver.GetQueueName(_exchangeName, _subscriberId);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
