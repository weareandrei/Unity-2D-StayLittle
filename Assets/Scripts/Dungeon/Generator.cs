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
        private static List<QuadrantLayout> _quadrantLayouts = GetQuadrantLayouts();
        // Once we have the roomsInstances -> we can make the Dungeon Map
        //  using the ID's filling them into the 2D array.


        public static void GenerateBySeed(string seed) {
            List<QuadrantLayout> quadrantsInUse = new List<QuadrantLayout>();
            List<DungeonMap.ExitData> exitsPending = new List<DungeonMap.ExitData>();
            // Dungeon consists of Quadrants (5x5 rooms)
            // Quadrant consists of RoomInstances

            // 2D Dungeon Map (inside DungeonData) will expand while Dungeon generates
            DungeonData dungeonData = new DungeonData();
            dungeonData.roomsAvailable = _roomsInstances; 
            // DungeonData must know the list of RoomInstance's to be able to apply them

            // In our Dungeon everything is based on Quadrants.
            // So we must have a List of Quadrant layouts.
            // We load the list and pick a random (appropriate) layout until we reach the  maxQuadrantsInDungeon
            QuadrantLayout entranceLayout = ChooseEntranceLayout();

            while (quadrantsInUse.Count < DungeonConsts.maxQuadrantsInDungeon
                   && exitsPending.Count > 0) { 
                // Find new Layouts appropriate to existing one's
                DungeonMap.ExitData thisExit = exitsPending[0];

                Vector2 currentQuadrantPos = findQuadrantPosition(dungeonData, thisExit);
                int[,] exitsLocated = dungeonData.GetNewQuadrantExitRequirements(currentQuadrantPos);
                // Now once we have exit pattern - we can find an appropriate Quadrant.

                QuadrantLayout newQuadrantLayout = ChooseQuadrantByExitPattern(exitsLocated);
                dungeonData = ConstructQuadrant(dungeonData, currentQuadrantPos, newQuadrantLayout);

                // We should take those exits because we will then easily
                //  miss other exits attached to new Quadrant.
                // Instead we should go around the new Quadrant and locate the exits there.

            }
        }

        private static Vector2 findQuadrantPosition(DungeonData dungeonData, DungeonMap.ExitData mainExit) {
            Vector2 currentQuadrantPos = default;
            switch (mainExit.exitDirection) {
                case DungeonMap.ExitDirection.Top:
                    currentQuadrantPos = dungeonData.FindCurrentQuadrantUsingCoordinate(mainExit.x, mainExit.y+1);
                    break;
                case DungeonMap.ExitDirection.Bottom:
                    currentQuadrantPos = dungeonData.FindCurrentQuadrantUsingCoordinate(mainExit.x, mainExit.y-1);
                    break;
                case DungeonMap.ExitDirection.Left:
                    currentQuadrantPos = dungeonData.FindCurrentQuadrantUsingCoordinate(mainExit.x-1, mainExit.y);
                    break;
                case DungeonMap.ExitDirection.Right:
                    currentQuadrantPos = dungeonData.FindCurrentQuadrantUsingCoordinate(mainExit.x+1, mainExit.y);
                    break;
                default:
                    Debug.Log("Current mainExit does not have an appropriate exitDirection specified");
                    break;
            }
            return currentQuadrantPos;
        }

        private static QuadrantLayout ChooseEntranceLayout() {
            // Should consider Seed
            foreach (QuadrantLayout layout in _quadrantLayouts) {
                if (layout.quadrantType == QuadrantType.Entrance) {
                    return layout;
                }
            }

            return _quadrantLayouts[0];
        }

        private static QuadrantLayout ChooseQuadrantByExitPattern(int[,] exitsPattern) {
            foreach (QuadrantLayout layout in _quadrantLayouts) {

                bool allExitsCorrespond = true;
                for (int y = 0; y < DungeonConsts.defaultQuadrantSize; y++) {
                    for (int x = 0; x < DungeonConsts.defaultQuadrantSize; x++) {
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

            return _quadrantLayouts[0];
        }

        private static DungeonData ConstructQuadrant(DungeonData dungeon, Vector2 currentQuadrantPos, QuadrantLayout newQuadrantLayout) {
            DungeonData extendedDungeon = dungeon;
            // Go though the Quadrant Grid by one room from bottom left to to right.
            //  1 row at a time.
            int quadrantStartX = (int)currentQuadrantPos.x * DungeonConsts.defaultQuadrantSize - 1;
            int quadrantStartY = (int)currentQuadrantPos.y * DungeonConsts.defaultQuadrantSize - 1;
            for (int y = 0; y < DungeonConsts.defaultQuadrantSize-1; y++) {
                for (int x = 0; x < DungeonConsts.defaultQuadrantSize-1; x++) {
                    if (newQuadrantLayout.rooms.GetCell(x, y) != 2) {
                        // Get random exit room here
                        Vector2 roomCoordinates = new Vector2(x,y);
                        extendedDungeon = ChooseExitRoom(extendedDungeon, roomCoordinates);
                    }
                    else {
                        // Get random room here
                    }
                    // They should both correspond to the entire newQuadrantConstruction schema
                    // : check neighbour cells in newQuadrantLayout to determine what
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
        
        private static List<QuadrantLayout> GetQuadrantLayouts() {
            const string layoutsPath = "Dungeon/Quadrant/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            QuadrantLayout[] quadrantLayouts = layoutsPrefabs.Select(
                prefabObj => prefabObj.GetComponent<QuadrantLayout>()
            ).ToArray();
            return new List<QuadrantLayout>(quadrantLayouts);
        }
    }
}