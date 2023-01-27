using UnityEngine;
using Dungeon.Generator;
using Dungeon.Properties.Map.Util;

namespace Dungeon.Properties.Map {
    public abstract class DungeonMap {
        public Grid2DResizable map = new Grid2DResizable(Consts.ChunkSize);

        public void PlaceCellOnMap(Vector2Int coordinates, string contents) {
            map.UpdateCell((int)coordinates[0], (int)coordinates[1], contents);
        }
    }
}