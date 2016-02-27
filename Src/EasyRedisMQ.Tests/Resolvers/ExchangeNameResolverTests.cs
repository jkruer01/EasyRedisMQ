using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyRedisMQ.Resolvers;
using System.Text;

namespace EasyRedisMQ.Tests.Resolvers
{
    [TestClass]
    public class ExchangeNameResolverTests
    {
        private ExchangeNameResolver _exchangeNameResolver;

        [TestInitialize]
        public void InitializeAll_ExchangeNameResolverTests()
        {
            _exchangeNameResolver = new ExchangeNameResolver();
        }

        [TestMethod]
        public void ExchangeNameResolver_GetExchangeName_ShouldReturn_ExchangePlusTypeName()
        {
            StringBuilder myObject = new StringBuilder();

            var expectedResult = "Exchange.System.Text.StringBuilder";
            var result = _exchangeNameResolver.GetExchangeName(myObject);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ExchangeNameResolver_GetExchangeName_Generic_ShouldReturn_ExchangePlusTypeName()
        {
            var expectedResult = "Exchange.System.Text.StringBuilder";
            var result = _exchangeNameResolver.GetExchangeName<StringBuilder>();

            Assert.AreEqual(expectedResult, result);
        }
    }
}
