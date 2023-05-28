using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Model {
    public abstract class DungeonMap {
        public Grid2DResizable map;

        public void PlaceCellOnMap(Vector2Int coordinates, string contents) {
            map.UpdateCell(coordinates.x, coordinates.y, contents);
        }

    }
}