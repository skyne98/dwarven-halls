using System;
using System.IO;
using System.Linq;
using ProtoBuf;
using TheDwarvenHalls.Shared.Messages.Network;

namespace TheDwarvenHalls.Shared.Messages
{
    [ProtoContract]
    [ProtoInclude(500, typeof(TestMessage))]
    [ProtoInclude(501, typeof(AuthenticationMessage))]
    public class Message
    {
        [ProtoMember(1)]
        public DateTime Time { get; set; }

        // The basic message body type
        public byte[] Serialize()
        {
            byte[] msgOut;
            int bytes;

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                bytes = (int) stream.Length;
                msgOut = stream.GetBuffer();
            }

            return msgOut.Take(bytes).ToArray();
        }

        public static T Deserialize<T>(byte[] message) where T: Message
        {
            T msgOut;

            using (var stream = new MemoryStream(message))
            {
                msgOut = Serializer.Deserialize<T>(stream);
            }

            return (T)msgOut;
        } 
        
        public static Message Deserialize(byte[] message)
        {
            return Deserialize<Message>(message);
        }   
    }
}