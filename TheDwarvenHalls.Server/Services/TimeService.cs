using System;
using Leopotam.Ecs;
using Serilog;

namespace TheDwarvenHalls.Server.Services
{
    public class TimeService: IEcsSystem, IEcsInitSystem, IEcsRunSystem
    {
        public TimeSpan RunTime { get; private set; }

        private DateTime _lastRun;
        
        public void Initialize()
        {
            _lastRun = DateTime.Now;
        }

        public void Destroy()
        {
            
        }
        
        public void Run()
        {
            RunTime = DateTime.Now - _lastRun;
            _lastRun = DateTime.Now;
        }
    }
}