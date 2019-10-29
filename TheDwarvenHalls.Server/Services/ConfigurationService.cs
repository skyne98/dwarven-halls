using System;
using System.IO;
using System.Text.Json.Serialization;
using Leopotam.Ecs;
using Newtonsoft.Json;
using Serilog;

namespace TheDwarvenHalls.Server.Services
{
    public class ConfigurationService: IEcsSystem
    {
        public Configuration Configuration { get; private set; }

        public ConfigurationService()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDirectory, "config.json");

            if (File.Exists(path))
            {
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
                Log.Information("Configuration read");
            }
            else
            {
                Configuration = new Configuration();
                var configJson = JsonConvert.SerializeObject(Configuration);
                File.WriteAllText(path, configJson);
                Log.Warning("Configuration file was missing, generated an empty one");
            }   
        }
    }
}