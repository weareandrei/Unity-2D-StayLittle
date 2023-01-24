using System.Collections.Generic;
using Dungeon.Chunk;
using Dungeon.Generator.Stage;
using Dungeon.Properties;
using Dungeon.Room;

namespace Dungeon.Generator {
    public static class Generator {
        
        public static List<ChunkLayout> ChunkLayoutsAvailable;
        public static List<RoomInstance> RoomsAvailable;
        
        public static DungeonData GenerateDungeonBySeed(string seed) {
            DungeonData dungeonData = new DungeonData();
            
            dungeonData.chunkMap = ChunkGenerator.GenerateChunks(seed);
            dungeonData.roomMap = RoomGenerator.GenerateRooms(seed, dungeonData.chunkMap);
            dungeonData.contentsMap = ContentsGenerator.GenerateContents(seed, dungeonData.roomMap);

            return dungeonData;
        }

        public static void LoadResources() {
            
        }
    }
}