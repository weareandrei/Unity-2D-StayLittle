using System.Collections.Generic;
using Dungeon.Model;
using HoneyGrid2D;
using Util;
using UnityEngine;

namespace Dungeon.Data {
    public static class DungeonListGenerator {
        
        public static string seedState;
        private static string _seedOriginalState;

        private static int lowestYOccupied_Left = 0;
        private static int lowestYOccupied_Right = 0;
        
        private static int dungeonIdCounter = 1;

        public static List<DungeonData> Generate(int count, string seed) {
            seedState = seed;
            _seedOriginalState = seed;
            List<DungeonData> generatedDungeonData = new List<DungeonData>();
            Generator.DungeonGenerator.LoadResources();
                
            for (int i = 0; i < count; i++) {
                generatedDungeonData.Add(GenerateNewDungeonData());
            }

            return generatedDungeonData;
        }
        
        private static DungeonData GenerateNewDungeonData() {
            string dungeonId = dungeonIdCounter.ToString();
            dungeonIdCounter++;

            string dungeonSeed = Seed.UseSeedToGenerateSeed(ref seedState);

            Vector2Int dungeonCoordinates = new Vector2Int(0, 0);
            
            int dungeonDirection = Seed.UseSeed(ref seedState, 100);
            if (dungeonDirection % 2 == 0) {
                dungeonCoordinates.x = -1; // left dungeon
            }
            if (dungeonDirection % 2 != 0) {
                dungeonCoordinates.x = 1; // right dungeon
            }

            Generator.DungeonGenerator.exitDirection =
                dungeonCoordinates.x == 1 ? Exit.SidePosition.Left : Exit.SidePosition.Right;
            DungeonMapData dungeonMapData = Generator.DungeonGenerator.GenerateDungeonBySeed(dungeonSeed);
            int dungeonHeight = dungeonMapData.roomMap.map.getYSize();
            int dungeonWidth = dungeonMapData.roomMap.map.getXSize();

            if (dungeonCoordinates.x == -1) {
                // This is the left coordinate
                dungeonCoordinates.y =  lowestYOccupied_Left + dungeonHeight * Consts.Get<int>("SizeOfRoom_PX");
                lowestYOccupied_Left += dungeonCoordinates.y;
            } if (dungeonCoordinates.x == 1) {
                // This is the left coordinate
                dungeonCoordinates.y = lowestYOccupied_Right + dungeonHeight * Consts.Get<int>("SizeOfRoom_PX");
                lowestYOccupied_Right += dungeonCoordinates.y;
            }
            
            return new DungeonData(
                dungeonId,
                dungeonCoordinates, 
                dungeonSeed, 
                dungeonWidth,
                SelectDungeonType());
        }

        private static _dungeonType SelectDungeonType() {
            int generatedNumber = Seed.UseSeed(ref seedState, 30);

            if (generatedNumber == 20 || generatedNumber == 30 || generatedNumber == 50) {
                return _dungeonType.bossDungeon;
            }
            if (generatedNumber == 19 || generatedNumber == 29 || generatedNumber == 49) {
                return _dungeonType.lostRoom;
            }
            if (generatedNumber % 3 == 0) {
                return _dungeonType.questDungeon;
            }

            return _dungeonType.regularDungeon;
        }
    }
}