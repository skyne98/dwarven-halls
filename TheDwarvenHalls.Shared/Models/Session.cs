using System;
using System.Net;
using LiteDB;

namespace TheDwarvenHalls.Shared.Models
{
    public class Session
    {
        public Guid Id { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public DateTime Time { get; set; }

        [BsonRef("users")] public User User { get; set; }
    }
}