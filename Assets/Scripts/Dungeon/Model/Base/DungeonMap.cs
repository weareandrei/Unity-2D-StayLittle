using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Model {
    public abstract class DungeonMap {
        public FlexGrid2DString map;
        
        public DungeonMap(int sizeX, int sizeY) {
            map = new FlexGrid2DString(sizeX, sizeY);
        }

        public DungeonMap ()  {
            map = new FlexGrid2DString(Consts.Get<int>("ChunkSize"), Consts.Get<int>("ChunkSize"));
        }

        public void PlaceCellOnMap(Vector2Int coordinates, string contents) {
            map.UpdateCell(coordinates.x, coordinates.y, contents);
        }

    }
}