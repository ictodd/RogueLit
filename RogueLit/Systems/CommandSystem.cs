﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueLite.Core;

namespace RogueLite.Systems {
    // Class to carry out commands the player executes
    public class CommandSystem {
        // Return value is true if the player was able to move
        // False when the player couldnt move, such as trying to move into a wall
        public bool MovePlayer(Direction direction) {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            switch (direction) {
                case Direction.Up:
                    y = Game.Player.Y - 1;
                    break;
                case Direction.Down:
                    y = Game.Player.Y + 1;
                    break;
                case Direction.Left:
                    x = Game.Player.X - 1;
                    break;
                case Direction.Right:
                    x = Game.Player.X + 1;
                    break;
                default:
                    return false;
            }
            if(Game.DungeonMap.SetActorPosition(Game.Player, x, y)) {
                return true;
            }
            return false;
        }
    }
}
