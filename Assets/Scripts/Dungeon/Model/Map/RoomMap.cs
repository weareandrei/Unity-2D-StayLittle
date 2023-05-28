using Dungeon.Generator;
using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Model {
    public class RoomMap : DungeonMap {
        public RoomMap(int sizeX, int sizeY) {
            map = new Grid2DResizable(sizeX, sizeY);
        }

        public RoomMap (int size) {
            map = new Grid2DResizable(size);
        }
        
        public RoomMap () {
            map = new Grid2DResizable(Consts.ChunkSize);
        }

        public Vector2Int CalculateSize() {
            return new Vector2Int(map.getXSize(), map.getYSize());
        }
        
        public Vector2Int GetEntrancePosition() {
            // todo: for now we assume that entrance can only be on the left side
            for (int i = 0; i < map.getYSize(); i++) {
                string thisRoomID = map.GetCellActual(0, i);
                if (RoomGenerator.FindRoomInstanceByID(thisRoomID).type == RoomType.Entrance) {
                    // Then this is the coordinate of entrance
                    return new Vector2Int(-1, i);
                }
            }

            return new Vector2Int(0, 0);
        }
    }
}