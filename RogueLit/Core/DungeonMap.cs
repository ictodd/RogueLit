using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueSharp;
using RLNET;

namespace RogueLite.Core {
    // extend RogueSharp's Map class to add functionality
    public class DungeonMap : Map{
        
        // draw method to be called each time the map is updated
        // it will render all of the symbols / colours for each cell to the map sub console
        public void Draw(RLConsole mapConsole) {
            mapConsole.Clear();
            foreach(Cell cell in GetAllCells()) {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell) {
            // if we havent explored a cell, we dont want to draw anything
            if (!cell.IsExplored)
                return;

            // when a cell is currently in the field-of-view it should be drawn with lighter colours
            if(IsInFov(cell.X, cell.Y)) {
                // choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable) {
                    console.Set(cell.X, cell.Y, Colours.FloorFov, Colours.FloorBackgroundFov, '.');
                } else {
                    console.Set(cell.X, cell.Y, Colours.WallFov, Colours.WallBackgroundFov, '#');
                }
            // when a cell is outside fov, draw it with darker colours
            } else {
                if (cell.IsWalkable) {
                    console.Set(cell.X, cell.Y, Colours.Floor, Colours.FloorBackground, '.');
                } else {
                    console.Set(cell.X, cell.Y, Colours.Wall, Colours.WallBackground, '#');
                }
            }
        }

        // This method will be called any time we move the player to update fov
        public void UpdatePlayerFieldOfView() {
            Player player = Game.Player;
            // Compute the fov based on the player's location and awareness
            ComputeFov(player.X, player.Y, player.Awareness, true);
            // Mark all cells in fov as having been explored
            foreach(Cell cell in GetAllCells()) {
                if (IsInFov(cell.X, cell.Y)) {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        // Returns true when able to place the Actor on the cell or false otherwise
        /* The SetActorPosition() method runs through a set of steps to make sure that the old cell that the actor was previously on is now walkable, 
         * and the new cell the actor is moved to is not walkable. It also updates the player field-of-view if the actor that was moved was the player. 
         * The return status here is also important. It returns false if the actor could not be moved. This is necessary when moving the player in case 
         * we try to move into a wall or other impassible cell.
         */
        public bool SetActorPosition(Actor actor, int x, int y) {
            // Only allow actor placement if the cell is walkable
            if (GetCell(x, y).IsWalkable) {
                // The cell the actor was previously on is now walkable
                SetIsWalkable(actor.X, actor.Y, true);
                // Update the actors position
                actor.X = x;
                actor.Y = y;
                // The new cell the actor is on is now not walkable
                SetIsWalkable(actor.X, actor.Y, false);
                // Dont forget to update the fov if we just repositioned the player
                if(actor is Player) {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        // Helper method or setting the IsWalkable property on a cell
        public void SetIsWalkable(int x, int y, bool isWalkable) {
            Cell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }
    }
}
