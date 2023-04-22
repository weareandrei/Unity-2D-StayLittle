using UnityEngine;
using Dungeon.Generator;
using Dungeon.Properties.Map.Util;

namespace Dungeon.Properties.Map {
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
            map.UpdateCell((int)coordinates[0], (int)coordinates[1], contents);
        }
        
        
    }
}