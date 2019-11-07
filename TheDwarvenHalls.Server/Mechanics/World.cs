using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Serilog;
using SimpleInjector;
using TheDwarvenHalls.Server.Database;
using TheDwarvenHalls.Server.Network;
using TheDwarvenHalls.Server.Services;

namespace TheDwarvenHalls.Server.Mechanics
{
    public class World: IDisposable
    {
        private readonly Context _context;
        private readonly EcsWorld _world;
        private readonly EcsSystems _systems;
        private readonly Container _container;
        private readonly Dictionary<Type, IEcsSystem> _registeredSystems;

        public World(Context context)
        {
            _context = context;
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _registeredSystems = new Dictionary<Type, IEcsSystem>();

            // CI
            _container = new Container();
            _container.Register(() => this, Lifestyle.Singleton);
            _container.Register(() => _world, Lifestyle.Singleton);
            _container.Register(() => _systems, Lifestyle.Singleton);
            _container.Register(() => _context, Lifestyle.Singleton);
            
            RegisterSystems();
            _systems.Initialize();
            
            Log.Information("World initialized");
        }

        private void RegisterSystems()
        {
            // Register systems here
            var toInstantiate = new List<Func<IEcsSystem>>
            {
                RegisterSystem<TimeService>(),
                RegisterSystem<ConfigurationService>(),
                RegisterSystem<ServerService>()
            };
            
            toInstantiate.ForEach(factory => factory());
            _container.Verify();
        }

        private Func<T> RegisterSystem<T>() where T: class, IEcsSystem
        {
            _container.Register<T>(Lifestyle.Singleton);
            return () =>
            {
                var system = _container.GetInstance<T>();
                _registeredSystems.Add(typeof(T), system);
                _systems.Add(system);

                var name = typeof(T).Name;
                var isService = name.ToLower().Contains("service");
                Log.Information($"Registered {(isService ? "service" : "system")} \"{name}\"");
                return system;
            };
        }

        public void Run()
        {
            _systems.Run();
        }

        public void Dispose()
        {
            _context?.Dispose();
            _container?.Dispose();
        }
    }
}