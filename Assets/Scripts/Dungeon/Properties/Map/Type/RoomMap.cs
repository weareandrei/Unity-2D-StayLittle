using Dungeon.Generator;
using Dungeon.Properties.Map.Util;

namespace Dungeon.Properties.Map.Type {
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
    }
}