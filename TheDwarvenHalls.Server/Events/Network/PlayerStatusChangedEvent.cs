namespace TheDwarvenHalls.Server.Events.Network
{
    public abstract class PlayerStatusChangedEvent: NetworkEvent
    {
        public string Name { get; set; }
    }
}