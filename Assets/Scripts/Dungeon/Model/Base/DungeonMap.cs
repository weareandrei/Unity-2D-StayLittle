using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Model {
    public abstract class DungeonMap {
        public Grid2DResizable map;
        
        public DungeonMap(int sizeX, int sizeY) {
            map = new Grid2DResizable(sizeX, sizeY);
        }

        public DungeonMap (int size) {
            map = new Grid2DResizable(size);
        }

        public DungeonMap ()  {
            map = new Grid2DResizable(Consts.ChunkSize);
        }

        public void PlaceCellOnMap(Vector2Int coordinates, string contents) {
            map.UpdateCell(coordinates.x, coordinates.y, contents);
        }

    }
}