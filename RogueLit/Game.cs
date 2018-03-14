using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;
using RogueLite.Core;
using RogueLite.Systems;


namespace RogueLite {
    public class Game {

        #region Console Variables

        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // the map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // below the map conole is the message console which displays attack rolls and other info
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // the stat console to the right of the map which displays player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // above the map is the inventory console
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        #endregion

        #region Misc Private Variables       

        private static string gameTitle = "Todd's Rogue Lite";
        private static string fontFile = "terminal8x8.png";
        private static float scaleFactor = 1.5f;
        private static bool _renderRequired = true;

        #endregion

        #region Public Variables

        public static Player Player { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static CommandSystem CommandSystem { get; private set; }

        #endregion

        public static void Main() {

            string fontFileName = fontFile;
            string consoleTitle = gameTitle;

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight,
                8, 8, scaleFactor, consoleTitle);

            // sub consoles to Bit Blit (bitmap combining)to
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            Player = new Player();
            // generate map
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            CommandSystem = new CommandSystem();

            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            // Refactored from OnRootConsoleUpdate - only needs to be called once to paint static consoles
            // The map console is handled when the dungeon map is drawn
            SetupSubConsoles();
            _rootConsole.Run();
        }

        #region Event Handlers

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e) {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if(keyPress != null) {
                switch (keyPress.Key) {
                    case RLKey.Up:
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                        break;
                    case RLKey.Down:
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                        break;
                    case RLKey.Left:
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                        break;
                    case RLKey.Right:
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                        break;
                    case RLKey.Escape:
                        _rootConsole.Close();
                        break;
                }
            }

            if (didPlayerAct)
                _renderRequired = true;

        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e) {

            BlitSubConsoles();
            // Dont bother redrawing all of the consoles if nothing has changed
            if (_renderRequired) {
                _rootConsole.Draw();
                DungeonMap.Draw(_mapConsole);
                Player.Draw(_mapConsole, DungeonMap);
                _renderRequired = false;
            }

        }

        #endregion

        private static void SetupSubConsoles() {
            // Removed the two lines below - handled in the DungeonMap.Draw() in the render event
            
            // _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colours.FloorBackground);
            // _mapConsole.Print(1, 1, "Map", Colours.TextHeading);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colours.TextHeading);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            _statConsole.Print(1, 1, "Stats", Colours.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colours.TextHeading);
        }

        private static void BlitSubConsoles() {

            // Map console - is below inventory and docked left
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            // Stat console is docked right
            RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
            // Message console is docked bottom
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            // Inventory console is docked top left
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
        }
    }
}
