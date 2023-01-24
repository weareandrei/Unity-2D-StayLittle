using System.Collections.Generic;
using Dungeon.Room;
using Dungeon.Chunk;
using UnityEngine;
using Dungeon.Properties.Map.Type;

namespace Dungeon.Properties {
    public class DungeonData {
        public ChunkMap chunkMap = new ChunkMap();
        public RoomMap roomMap = new RoomMap();
        public ContentsMap contentsMap = new ContentsMap();

        /*
         * ChunkMap is a 2D array. Where each cell has a string with the ChunkLayoutID
         * RoomMap is a 2D array. Where each cell has a string with the RoomInstanceID
         * ContentsMap is a 2D array. Where each cell has a ...
         */
    }
}