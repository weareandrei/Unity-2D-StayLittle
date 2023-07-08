using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Dungeon.Model;
using UnityEngine;
using Util;
using Global;

namespace Dungeon.Generator {
    public static class DungeonGenerator {
        
        public static List<Chunk> chunkLayoutsAvailable;
        public static GameObject[] roomPrefabsAvailable;
        public static List<Room> roomsAvailable;
        
        public static List<Vector2Int> roomMapEntrances;

        public static Exit.SidePosition exitDirection;
        
        public static string seedState;
        private static string _seedOriginalState;
        
        public static DungeonMapData GenerateDungeonBySeed(string seedGiven) {
            DungeonMapData dungeonMapData = new DungeonMapData();
            seedState = seedGiven;
            _seedOriginalState = seedGiven;

            ChunkGenerator.chunkLayoutsAvailable = chunkLayoutsAvailable;
            dungeonMapData.chunkMap = ChunkGenerator.GenerateChunks();
            Seed.ResetSeed(ref seedState, _seedOriginalState);
            
            RoomGenerator.roomLayoutsAvailable = roomsAvailable;
            dungeonMapData.roomMap = RoomGenerator.GenerateRooms(dungeonMapData.chunkMap);
            Seed.ResetSeed(ref seedState, _seedOriginalState);
            
            dungeonMapData.contentsMap = ContentsGenerator.GenerateContents(dungeonMapData.roomMap);

            return dungeonMapData;
        }

        public static void LoadResources() {
            string resourcesDirectory = GlobalVariables.ResourcesDirectory;
            chunkLayoutsAvailable = LoadAvailableChunkLayout(resourcesDirectory);
            roomPrefabsAvailable = LoadAvailableRooms(resourcesDirectory);
            roomsAvailable = LoadAvailableRoomsLayout();
        }
        
        private static List<Chunk> LoadAvailableChunkLayout(string dirBase) {
            string layoutsPath = dirBase + "Dungeon/ChunkLayout/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            Chunk[] chunkLayouts = layoutsPrefabs.Select(
                prefabObj => prefabObj.GetComponent<Chunk>()
            ).ToArray();
            return new List<Chunk>(chunkLayouts);
        }
        
        private static List<Room> LoadAvailableRoomsLayout() {
            Room[] roomInstances = roomPrefabsAvailable.Select(
                prefabObj => prefabObj.GetComponent<Room>()
            ).ToArray();
            return new List<Room>(roomInstances);
        }
        
        private static GameObject[] LoadAvailableRooms(string dirBase) {
            string layoutsPath = dirBase + "Dungeon/Room/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            return layoutsPrefabs;
        }

        public static GameObject GetRoomPrefabFromID(string id) {
            return roomPrefabsAvailable.FirstOrDefault(roomPrefab => {
                Room room = roomPrefab.GetComponent<Room>();
                if (room.roomID == id) {
                    return roomPrefab;
                }

                return false;
            });
        }
        
        public static int UseSeed(int choiceCount) {
            return Seed.UseSeed(ref seedState, choiceCount);
        }

    }
}