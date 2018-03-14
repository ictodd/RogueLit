using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;
using RogueLite.Interfaces;
using RogueLite.Systems;
using RogueSharp;

namespace RogueLite.Core {
    public class Actor : IActor, IDrawable {

        // IActor
        public string Name { get; set; }
        public int Awareness { get; set; }

        // IDrawable
        public RLColor Colour { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole console, IMap map) {
            // dont draw actors in cells that havent been explored
            if (!map.GetCell(X, Y).IsExplored)
                return;

            // Only draw the actor with the colour and symbol when they are in fov
            if (map.IsInFov(X, Y)) {
                console.Set(X, Y, Colour, Colours.FloorBackgroundFov, Symbol);
            } else {
                // When not in fov, just draw a normal floor
                console.Set(X, Y, Colours.Floor, Colours.FloorBackground, '.');
            }
        }
    }
}
