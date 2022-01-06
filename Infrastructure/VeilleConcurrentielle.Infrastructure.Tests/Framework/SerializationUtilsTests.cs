using System;
using System.Text.Json;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.TestLib.TestData;
using Xunit;

namespace VeilleConcurrentielle.Infrastructure.Tests.Framework
{
    public class SerializationUtilsTests
    {
        [Fact]
        public void Serialize_Deserialize()
        {
            SimpleClass source = new SimpleClass()
            {
                StringValue = "string",
                IntValue = 58,
                DoubleValue = 4.5,
                DateTimeValue = DateTime.Now
            };
            var serialized = SerializationUtils.Serialize(source);
            Assert.NotNull(serialized);
            var deserialized = SerializationUtils.Deserialize<SimpleClass>(serialized);
            Assert.NotNull(deserialized);
            Assert.Equal(source.StringValue, deserialized.StringValue);
            Assert.Equal(source.IntValue, deserialized.IntValue);
            Assert.Equal(source.DateTimeValue, deserialized.DateTimeValue);
            Assert.Equal(source.DoubleValue, deserialized.DoubleValue);
        }

        [Fact]
        public void Serialize_Deserialize_WithSubClassFromAbstract()
        {
            Container<Child1> source = new Container<Child1>()
            {
                ContainerName = "Container",
                Child = new Child1() { IntValue = 5 }
            };
            var serialized = SerializationUtils.Serialize(source);
            Assert.NotNull(serialized);
            var deserialized = SerializationUtils.Deserialize<Container<Child1>>(serialized);
            Assert.NotNull(deserialized);
            Assert.Equal(source.ContainerName, deserialized.ContainerName);
            Assert.Equal(source.Child.GetType(), deserialized.Child.GetType());
            Assert.Equal(source.Child.ChildName, deserialized.Child.ChildName);
            Assert.Equal(source.Child.IntValue, deserialized.Child.IntValue);
        }

        [Fact]
        public void Deserialize_WithMismatchType_ThrowsException()
        {
            var serialized = "Unknown type";
            Assert.ThrowsAny<JsonException>(() =>
            {
                SerializationUtils.Deserialize(serialized, typeof(SimpleClass));
            });
        }

        [Theory]
        [ClassData(typeof(EmptyStringTestData))]
        public void Deserialize_WithNullOrEmpty_ReturnsNull(string serialized)
        {
            var deserialized = SerializationUtils.Deserialize<SimpleClass>(serialized);
            Assert.Null(deserialized);
        }

        class SimpleClass
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
            public double DoubleValue { get; set; }
            public DateTime DateTimeValue { get; set; }
        }

        abstract class AbstractClass
        {
            public abstract string ChildName { get; }
            public int IntValue { get; set; }
        }

        class Child1 : AbstractClass
        {
            public override string ChildName => "Child1";
        }

        class Container<TChild> where TChild : AbstractClass
        {
            public string ContainerName { get; set; }
            public TChild Child { get; set; }
        }
    }
}
