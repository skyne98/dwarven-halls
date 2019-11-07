using System;
using Console = SadConsole.Console;

namespace TheDwarvenHalls
{
    public enum ScreenStatus
    {
        Created,
        Running,
        Paused,
        Stopped
    }
    
    /// <summary>
    /// A game screen, that represents the current scene being processed and rendered
    /// </summary>
    public abstract class Screen
    {
        private Console _uiConsole;
        private Console _gameConsole;

        public ScreenStatus Status { get; set; } = ScreenStatus.Created;

        protected Screen(Console uiConsole, Console gameConsole)
        {
            _uiConsole = uiConsole;
            _gameConsole = gameConsole;
        }

        public abstract void OnStarted();
        public abstract void OnStopped();
        public abstract void OnPaused();
        public abstract void OnResumed();
        public abstract void OnRun(TimeSpan frameTime);
        public abstract void OnDraw(TimeSpan frameTime);
    }
}