using System.Collections.Generic;
using System.Linq;
using Dungeon.Chunk;
using Dungeon.Generator.Stage;
using Dungeon.Properties;
using Dungeon.Room;
using UnityEngine;

namespace Dungeon.Generator {
    public static class Generator {
        
        public static List<ChunkLayout> chunkLayoutsAvailable;
        public static GameObject[] roomPrefabsAvailable;
        public static List<RoomInstance> roomsAvailable;
        
        public static DungeonMapData GenerateDungeonBySeed(string seed) {
            DungeonMapData dungeonMapData = new DungeonMapData();

            ChunkGenerator.chunkLayoutsAvailable = chunkLayoutsAvailable;
            RoomGenerator.roomLayoutsAvailable = roomsAvailable;
            ChunkGenerator.seed = seed;
            dungeonMapData.chunkMap = ChunkGenerator.GenerateChunks();
            dungeonMapData.roomMap = RoomGenerator.GenerateRooms(seed, dungeonMapData.chunkMap);
            dungeonMapData.contentsMap = ContentsGenerator.GenerateContents(seed, dungeonMapData.roomMap);

            return dungeonMapData;
        }

        public static void LoadResources() {
            chunkLayoutsAvailable = LoadAvailableChunkLayout();
            roomPrefabsAvailable = LoadAvailableRooms();
            roomsAvailable = LoadAvailableRoomsLayout();
        }
        
        private static List<ChunkLayout> LoadAvailableChunkLayout() {
            const string layoutsPath = "Dungeon/ChunkLayout/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            ChunkLayout[] chunkLayouts = layoutsPrefabs.Select(
                prefabObj => prefabObj.GetComponent<ChunkLayout>()
            ).ToArray();
            return new List<ChunkLayout>(chunkLayouts);
        }
        
        private static List<RoomInstance> LoadAvailableRoomsLayout() {
            RoomInstance[] roomInstances = roomPrefabsAvailable.Select(
                prefabObj => prefabObj.GetComponent<RoomInstance>()
            ).ToArray();
            return new List<RoomInstance>(roomInstances);
        }
        
        private static GameObject[] LoadAvailableRooms() {
            const string layoutsPath = "Dungeon/Room/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            return layoutsPrefabs;
        }

        public static GameObject GetRoomPrefabFromID(string id) {
            return roomPrefabsAvailable.FirstOrDefault(roomPrefab => {
                RoomInstance roomInstance = roomPrefab.GetComponent<RoomInstance>();
                if (roomInstance.roomID == id) {
                    return roomPrefab;
                }

                return false;
            });
        }
    }
}