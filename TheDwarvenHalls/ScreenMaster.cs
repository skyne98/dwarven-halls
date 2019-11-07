using System;
using System.Collections.Generic;

namespace TheDwarvenHalls
{
    public class ScreenMaster
    {
        private Stack<Screen> _screens;

        public ScreenMaster(Screen initialScreen)
        {
            _screens = new Stack<Screen>();
        }

        public void Push(Screen screen)
        {
            if (screen is null)
            {
                throw new ArgumentException("Provided screen is null");
            }
            if (_screens.Contains(screen))
            {
                throw new ArgumentException($"{screen.ToString()} is already on the stack");
            }

            var current = _screens.Peek();
            current?.OnPaused();
            _screens.Push(screen);
            screen.
        }
    }
}