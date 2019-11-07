using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace TheDwarvenHalls
{
    internal class Program
    {
        public const int Width = 80;
        public const int Height = 25;

        private static Console _console;
        private static FontMaster _kenneyFont;
        private static FontMaster _kenneyFontCharacters;
        private static FontMaster _cheepicusFont;
        private static Console _uiConsole;

        private static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create(Width, Height);
            Settings.ResizeMode = Settings.WindowResizeOptions.None;
            Settings.WindowMinimumSize = new Point(160, 160);
            ((Game) Game.Instance).WindowResized += Program_WindowResized;

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Load the fonts
            _kenneyFont = Global.LoadFont("Fonts/Kenney/Kenney.font");
            _kenneyFontCharacters = Global.LoadFont("Fonts/Kenney/KenneyCharacters.font");
            _cheepicusFont = Global.LoadFont("Fonts/DwarfFortress/Cheepicus.font");
            
            _console = new Console(Width, Height, _kenneyFont.GetFont(Font.FontSizes.One));
            _uiConsole = new Console(Width, Height, _cheepicusFont.GetFont(Font.FontSizes.One));

            _uiConsole.Parent = _console;

            // Any startup code for your game. We will use an example console for now
            _uiConsole.FillWithRandomGarbage();
            _uiConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0, SpriteEffects.None);
            _uiConsole.Print(6, 5, "Hello from SadConsole", ColorAnsi.CyanBright);

            Global.CurrentScreen = _console;
        }

        private static void Program_WindowResized(object? sender, EventArgs e)
        {
            var windowWidth = Global.WindowWidth > 0 ? Global.WindowWidth : 1;
            var windowHeight = Global.WindowHeight > 0 ? Global.WindowHeight : 1;
            Global.CurrentScreen.Resize(
                windowWidth / Global.CurrentScreen.Font.Size.X,
                windowHeight / Global.CurrentScreen.Font.Size.Y, 
                true);
            _uiConsole.Resize(
                windowWidth / Global.CurrentScreen.Font.Size.X,
                windowHeight / Global.CurrentScreen.Font.Size.Y, 
                true);

            for (int y = 0; y < Global.CurrentScreen.Height; y++)
            {
                for (int x = 0; x < Global.CurrentScreen.Width; x++)
                {
                    var index = x + y * Global.CurrentScreen.Width;
                    if (index < Global.CurrentScreen.Font.MaxGlyphIndex)
                        Global.CurrentScreen.SetGlyph(x, y, index, Color.White, Color.Black, SpriteEffects.None);   
                    if (index >= Global.CurrentScreen.Font.MaxGlyphIndex && index < _cheepicusFont.GetFont(Font.FontSizes.One).MaxGlyphIndex + Global.CurrentScreen.Font.MaxGlyphIndex)
                        _uiConsole.SetGlyph(x, y, index - Global.CurrentScreen.Font.MaxGlyphIndex, Color.Red, Color.Transparent, SpriteEffects.None);
                }
            }
        }
    }
}