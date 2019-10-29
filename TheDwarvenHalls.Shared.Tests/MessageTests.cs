using System;
using ProtoBuf;
using TheDwarvenHalls.Shared.Messages;
using Xunit;

namespace TheDwarvenHalls.Shared.Tests
{
    public class MessageTests
    {
        [Fact]
        public void TestMessageSerializesAndDeserializes()
        {
            var message = new TestMessage()
            {
                Time = DateTime.Now,
                SomeText = "SomeSpecificTestText"
            };

            var serialized = message.Serialize();
            var deserialized = Message.Deserialize(serialized);
            
            Assert.IsType<TestMessage>(deserialized);
        }
        
        [Fact]
        public void TestMessageSerializesAndDeserializesWithText()
        {
            var message = new TestMessage()
            {
                Time = DateTime.Now,
                SomeText = "SomeSpecificTestText"
            };

            var serialized = message.Serialize();
            var deserialized = Message.Deserialize<TestMessage>(serialized);
            
            Assert.IsType<TestMessage>(deserialized);
            Assert.Equal(message.SomeText, deserialized.SomeText);
        }
    }
}