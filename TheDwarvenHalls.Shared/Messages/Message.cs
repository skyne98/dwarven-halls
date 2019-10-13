using System;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace TheDwarvenHalls.Shared.Messages
{
    [ProtoContract]
    public abstract class AMessage
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

        public static T Deserialize<T>(byte[] message) where T: AMessage
        {
            AMessage msgOut;

            using (var stream = new MemoryStream(message))
            {
                msgOut = Serializer.Deserialize<AMessage>(stream);
            }

            return (T)msgOut;
        } 
        
        public static AMessage Deserialize(byte[] message)
        {
            return Deserialize<AMessage>(message);
        }   
    }
}