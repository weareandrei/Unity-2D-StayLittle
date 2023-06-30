using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Data {
    public static class DungeonList
    {
        public static List<DungeonData> _dungeons = new List<DungeonData>() {
            {new DungeonData("a",new Vector2Int(1, 100),"12345", 10, _dungeonType.regularDungeon)},
            {new DungeonData("b",new Vector2Int(1, 200),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("c",new Vector2Int(1, 300),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("d",new Vector2Int(1, 400),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("e",new Vector2Int(1, 500),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("f",new Vector2Int(1, 600),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("g",new Vector2Int(1, 700),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("h",new Vector2Int(1, 800),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("i",new Vector2Int(1, 900),"12345", 10,_dungeonType.regularDungeon)},
            {new DungeonData("j",new Vector2Int(1, 1000),"12345", 10,_dungeonType.regularDungeon)}
        }; // Assume its sorted already
        
        public static void Initialize(string seed) {
            int maxDungeons = Consts.Get<int>("MaxDungeons");
        }

        public static int CountDungeons() {
            return _dungeons.Count;
        }

        public static DungeonData GetDungeonDataByIndex(int index) {
            return _dungeons[index];
        }

        public static void SaveDungeons() {

        }

        public static void LoadDungeons() {

        }
    }
    
    [Serializable]
    public struct DungeonData {
        [SerializeField]
        public string id;
        [SerializeField]
        public Vector2Int coordinates;
        [SerializeField]
        public string seed;
        [SerializeField]
        public int dungeonWidth; // Including only non-empty columns
        [SerializeField]
        public _dungeonType typeOfDungeon;

        public DungeonData(string id, Vector2Int coordinates, string seed, int dungeonWidth, _dungeonType typeOfDungeon) {
            this.id = id;
            this.coordinates = coordinates;
            this.seed = seed;
            this.dungeonWidth = dungeonWidth;
            this.typeOfDungeon = typeOfDungeon;
        }
    }
    
    public enum _dungeonType {
        lostRoom,
        bossDungeon,
        questDungeon,
        regularDungeon
    }
}
