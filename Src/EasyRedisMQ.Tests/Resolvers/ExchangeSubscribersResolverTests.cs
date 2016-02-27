using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyRedisMQ.Resolvers;
using Moq;

namespace EasyRedisMQ.Tests.Resolvers
{
    [TestClass]
    public class ExchangeSubscribersResolverTests
    {
        private ExchangeSubscribersResolver _exchangeSubscribersResolver;
        private Mock<IExchangeNameResolver> _exchangeNameResolver;
        private string exchangeName = "exchangeName";

        [TestInitialize]
        public void InitializeAll_ExchangeSubscribersResolverTests()
        {
            _exchangeNameResolver = new Mock<IExchangeNameResolver>();
            _exchangeNameResolver.Setup(m => m.GetExchangeName(It.IsAny<string>())).Returns(exchangeName);
            _exchangeNameResolver.Setup(m => m.GetExchangeName(It.IsAny<object>())).Returns(exchangeName);
            _exchangeNameResolver.Setup(m => m.GetExchangeName<object>()).Returns(exchangeName);
            _exchangeSubscribersResolver = new ExchangeSubscribersResolver(_exchangeNameResolver.Object);
        }

        [TestMethod]
        public void ExchangeSubscribersResolver_GetSubscriberKey_Object_ShouldReturnExchangeNamePlusSubscribers()
        {
            var expectedResult = exchangeName + ".Subscribers";
            var result = _exchangeSubscribersResolver.GetSubscriberKey(new object());

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ExchangeSubscribersResolver_GetSubscriberKey_String_ShouldReturnExchangeNamePlusSubscribers()
        {
            var expectedResult = exchangeName + ".Subscribers";
            var result = _exchangeSubscribersResolver.GetSubscriberKey(exchangeName);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ExchangeSubscribersResolver_GetSubscriberKey_Generic_ShouldReturnExchangeNamePlusSubscribers()
        {
            var expectedResult = exchangeName + ".Subscribers";
            var result = _exchangeSubscribersResolver.GetSubscriberKey<object>();

            Assert.AreEqual(expectedResult, result);
        }
    }
}
