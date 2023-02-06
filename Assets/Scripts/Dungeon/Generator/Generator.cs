using System.Collections.Generic;
using System.Linq;
using Dungeon.Chunk;
using Dungeon.Generator.Stage;
using Dungeon.Properties;
using Dungeon.Room;
using UnityEngine;

namespace Dungeon.Generator {
    public static class Generator {
        
        public static List<ChunkLayout> ChunkLayoutsAvailable;
        public static List<RoomInstance> RoomsAvailable;
        
        public static DungeonData GenerateDungeonBySeed(string seed) {
            DungeonData dungeonData = new DungeonData();

            ChunkGenerator.chunkLayoutsAvailable = ChunkLayoutsAvailable;
            dungeonData.chunkMap = ChunkGenerator.GenerateChunks();
            dungeonData.roomMap = RoomGenerator.GenerateRooms(seed, dungeonData.chunkMap);
            dungeonData.contentsMap = ContentsGenerator.GenerateContents(seed, dungeonData.roomMap);

            return dungeonData;
        }

        public static void LoadResources() {
            const string layoutsPath = "Dungeon/Chunk/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            ChunkLayout[] chunkLayouts = layoutsPrefabs.Select(
                prefabObj => prefabObj.GetComponent<ChunkLayout>()
            ).ToArray();
            ChunkLayoutsAvailable = new List<ChunkLayout>(chunkLayouts);
        }
    }
}