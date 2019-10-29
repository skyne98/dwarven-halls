using ProtoBuf;

namespace TheDwarvenHalls.Shared.Messages
{
    [ProtoContract]
    public class TestMessage: Message
    {
        [ProtoMember(1)]
        public string SomeText { get; set; }

        public TestMessage()
        {
            
        }
    }
}