using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using Dungeon.Room;
using Dungeon.Properties;
using Dungeon.Properties.MapUtilities;
using Dungeon.Quadrant;
using Vector2 = UnityEngine.Vector2;

namespace Dungeon {
    
    public static class Generator {
        private static List<RoomInstance> _roomsInstances = GetRoomInstances();
        private static List<ChunkLayout> _chunkLayouts = GetChunkLayouts();
        // Once we have the roomsInstances -> we can make the Dungeon Map
        //  using the ID's filling them into the 2D array.


        public static void GenerateBySeed(string seed) {
            List<ChunkLayout> chunksInUse = new List<ChunkLayout>();
            List<DungeonMap.ExitData> exitsPending = new List<DungeonMap.ExitData>();
            // Dungeon consists of Chunks (5x5 rooms)
            // Chunk consists of RoomInstances

            // 2D Dungeon Map (inside DungeonData) will expand while Dungeon generates
            DungeonData dungeonData = new DungeonData();
            dungeonData.roomsAvailable = _roomsInstances; 
            // DungeonData must know the list of RoomInstance's to be able to apply them

            // In our Dungeon everything is based on Chunks.
            // So we must have a List of Chunk layouts.
            // We load the list and pick a random (appropriate) layout until we reach the  maxChunksInDungeon
            ChunkLayout entranceLayout = ChooseEntranceLayout();

            while (chunksInUse.Count < DungeonConsts.maxChunksInDungeon
                   && exitsPending.Count > 0) { 
                // Find new Layouts appropriate to existing one's
                DungeonMap.ExitData thisExit = exitsPending[0];

                Vector2 currentChunkPos = FindChunkPosition(dungeonData, thisExit);
                int[,] exitsLocated = dungeonData.GetNewChunkExitRequirements(currentChunkPos);
                // Now once we have exit pattern - we can find an appropriate Chunk.

                ChunkLayout newChunkLayout = ChooseChunkByExitPattern(exitsLocated);
                dungeonData = ConstructChunk(dungeonData, currentChunkPos, newChunkLayout);

                // We should take those exits because we will then easily
                //  miss other exits attached to new Chunk.
                // Instead we should go around the new Chunk and locate the exits there.

            }
        }

        private static Vector2 FindChunkPosition(DungeonData dungeonData, DungeonMap.ExitData mainExit) {
            Vector2 currentChunkPos = default;
            switch (mainExit.exitDirection) {
                case DungeonMap.ExitDirection.Top:
                    currentChunkPos = dungeonData.FindCurrentChunkUsingCoordinate(mainExit.x, mainExit.y+1);
                    break;
                case DungeonMap.ExitDirection.Bottom:
                    currentChunkPos = dungeonData.FindCurrentChunkUsingCoordinate(mainExit.x, mainExit.y-1);
                    break;
                case DungeonMap.ExitDirection.Left:
                    currentChunkPos = dungeonData.FindCurrentChunkUsingCoordinate(mainExit.x-1, mainExit.y);
                    break;
                case DungeonMap.ExitDirection.Right:
                    currentChunkPos = dungeonData.FindCurrentChunkUsingCoordinate(mainExit.x+1, mainExit.y);
                    break;
                default:
                    Debug.Log("Current mainExit does not have an appropriate exitDirection specified");
                    break;
            }
            return currentChunkPos;
        }

        private static ChunkLayout ChooseEntranceLayout() {
            // Should consider Seed
            foreach (ChunkLayout layout in _chunkLayouts) {
                if (layout.chunkType == ChunkType.Entrance) {
                    return layout;
                }
            }

            return _chunkLayouts[0];
        }

        private static ChunkLayout ChooseChunkByExitPattern(int[,] exitsPattern) {
            foreach (ChunkLayout layout in _chunkLayouts) {

                bool allExitsCorrespond = true;
                for (int y = 0; y < DungeonConsts.defaultChunkSize; y++) {
                    for (int x = 0; x < DungeonConsts.defaultChunkSize; x++) {
                        if (layout.rooms.GetCell(x, y) != 2 ||
                            exitsPattern[x, y] != 1) {
                            allExitsCorrespond = false;
                        }
                    }
                }

                if (allExitsCorrespond) {
                    // Should actually be done by SEED. For example if SEED CHAR is 2,
                    //  then second time we enter here - we return.
                    return layout;
                }
            }

            return _chunkLayouts[0];
        }

        private static DungeonData ConstructChunk(DungeonData dungeon, Vector2 currentChunkPos, ChunkLayout newChunkLayout) {
            DungeonData extendedDungeon = dungeon;
            // Go though the Chunk Grid by one room from bottom left to to right.
            //  1 row at a time.
            int chunkStartX = (int)currentChunkPos.x * DungeonConsts.defaultChunkSize - 1;
            int chunkStartY = (int)currentChunkPos.y * DungeonConsts.defaultChunkSize - 1;
            for (int y = 0; y < DungeonConsts.defaultChunkSize-1; y++) {
                for (int x = 0; x < DungeonConsts.defaultChunkSize-1; x++) {
                    if (newChunkLayout.rooms.GetCell(x, y) != 2) {
                        // Get random exit room here
                        Vector2 roomCoordinates = new Vector2(x,y);
                        extendedDungeon = ChooseExitRoom(extendedDungeon, roomCoordinates);
                    }
                    else {
                        // Get random room here
                    }
                    // They should both correspond to the entire newChunkConstruction schema
                    // : check neighbour cells in newChunkLayout to determine what
                    //    walls are required to be in the room
                }
            }

            return extendedDungeon;
        }

        private static DungeonData ChooseExitRoom(DungeonData dungeon, Vector2 roomCoordinates) {
            
        }

        private static List<RoomInstance> GetRoomInstances() {
            const string instancesPath = "Dungeon/Room/Prefabs";
            GameObject[] instancesPrefabs = Resources.LoadAll<GameObject>(instancesPath);
            RoomInstance[] instancesRooms = instancesPrefabs.Select(
                prefabObj => prefabObj.GetComponent<RoomInstance>()
            ).ToArray();
            return new List<RoomInstance>(instancesRooms);
        }
        
        private static List<ChunkLayout> GetChunkLayouts() {
            const string layoutsPath = "Dungeon/Chunk/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            ChunkLayout[] chunkLayouts = layoutsPrefabs.Select(
                prefabObj => prefabObj.GetComponent<ChunkLayout>()
            ).ToArray();
            return new List<ChunkLayout>(chunkLayouts);
        }
    }
}