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
        
        public static string seedState;
        private static string _seedOriginalState;
        
        public static DungeonMapData GenerateDungeonBySeed(string seedGiven) {
            DungeonMapData dungeonMapData = new DungeonMapData();
            seedState = seedGiven;
            _seedOriginalState = seedGiven;

            ChunkGenerator.chunkLayoutsAvailable = chunkLayoutsAvailable;
            dungeonMapData.chunkMap = ChunkGenerator.GenerateChunks();
            ResetSeed();
            
            RoomGenerator.roomLayoutsAvailable = roomsAvailable;
            dungeonMapData.roomMap = RoomGenerator.GenerateRooms(dungeonMapData.chunkMap);
            ResetSeed();
            
            dungeonMapData.contentsMap = ContentsGenerator.GenerateContents(dungeonMapData.roomMap);

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
        
        public static int UseSeed(int choiceCount) {
            int seedNumber = (int)seedState[0];
            seedState = seedState.Substring(1, seedState.Length - 1) + seedNumber;

            while (choiceCount <= seedNumber) {
                seedNumber = seedNumber - choiceCount;
                if (seedNumber < 0) {
                    seedNumber = 0;
                }
            }
            
            return seedNumber;
        }

        public static void ResetSeed() {
            seedState = _seedOriginalState;
        }
        
    }
}