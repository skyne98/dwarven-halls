using System;
using System.Collections.Generic;
using LiteDB;

namespace TheDwarvenHalls.Shared.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        [BsonRef("sessions")] public List<Session> Sessions { get; set; }
    }
}