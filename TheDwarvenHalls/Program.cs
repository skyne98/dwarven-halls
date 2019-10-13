using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using Game = SadConsole.Game;

namespace TheDwarvenHalls
{
    internal class Program
    {
        public const int Width = 80;
        public const int Height = 25;

        private static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            Settings.ResizeMode = Settings.WindowResizeOptions.None;
            ((Game) Game.Instance).WindowResized += Program_WindowResized;

            // Any startup code for your game. We will use an example console for now
            var startingConsole = Global.CurrentScreen;
            startingConsole.FillWithRandomGarbage();
            startingConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0, SpriteEffects.None);
            startingConsole.Print(6, 5, "Hello from SadConsole", ColorAnsi.CyanBright);
        }

        private static void Program_WindowResized(object sender, EventArgs e)
        {
            Global.CurrentScreen.Resize(Global.WindowWidth / Global.CurrentScreen.Font.Size.X,
                Global.WindowHeight / Global.CurrentScreen.Font.Size.Y, false);
            Global.CurrentScreen.FillWithRandomGarbage();
        }
    }
}