using ProtoBuf;

namespace TheDwarvenHalls.Shared.Messages.Network
{
    [ProtoContract]
    public class AuthenticationMessage: Message
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public string Password { get; set; }
    }
}