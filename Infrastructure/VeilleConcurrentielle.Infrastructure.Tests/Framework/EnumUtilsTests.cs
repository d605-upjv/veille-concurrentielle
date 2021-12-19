using System;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.Infrastructure.Tests.Framework
{
    public class EnumUtilsTests
    {
        enum TestEnum
        {
            One,
            Two
        }

        [Fact]
        public void GetValueFromString_WithCorrectValue()
        {
            var value = EnumUtils.GetValueFromString<TestEnum>("One");
            Assert.Equal(TestEnum.One, value);
        }

        [Fact]
        public void GetValueFromString_WithIncorrectValue_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                EnumUtils.GetValueFromString<TestEnum>("Three");
            });
        }
    }
}
