namespace TheDwarvenHalls.Server
{
    public class Configuration
    {
        public NetworkConfiguration Network { get; private set; } = new NetworkConfiguration();
    }

    public class NetworkConfiguration
    {
        public int? Port { get; private set; }
        public string Password { get; private set; }
    }
}