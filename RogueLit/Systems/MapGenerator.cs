using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueLite.Core;
using RogueSharp;

namespace RogueLite.Systems {
    public class MapGenerator {

        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        public MapGenerator(int width, int height) {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        // generate simple open floor map with walls around the outside
        public DungeonMap CreateMap() {
            // init every cell in map by setting
            // walkable, transparency, and explored to true
            _map.Initialize(_width, _height);
            foreach(Cell cell in _map.GetAllCells()) {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            // set the first and last rows in map to be not transparent and not walkable
            foreach(Cell cell in _map.GetCellsInRows(0, _height - 1)) {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            // set the first and last columns in map to be not transparent and not walkable
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1)) {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }

    }
}
