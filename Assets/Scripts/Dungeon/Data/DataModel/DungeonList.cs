using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Data {
    public static class DungeonList
    {
        public static List<DungeonData> _dungeons = new List<DungeonData>() {
            {new DungeonData("a",new Vector2Int(1, 100), new List<Vector2Int>(), "12345",10, DungeonType.regularDungeon)},
            {new DungeonData("b",new Vector2Int(1, 200), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("c",new Vector2Int(1, 300), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("d",new Vector2Int(1, 400), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("e",new Vector2Int(1, 500), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("f",new Vector2Int(1, 600), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("g",new Vector2Int(1, 700), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("h",new Vector2Int(1, 800), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("i",new Vector2Int(1, 900), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)},
            {new DungeonData("j",new Vector2Int(1, 1000), new List<Vector2Int>(), "12345",10,DungeonType.regularDungeon)}
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

        public static DungeonData GetDungeonDataByID(string id) {
            foreach (DungeonData dungeonData in _dungeons) {
                if (dungeonData.id == id) {
                    return dungeonData;
                }
            }

            throw new Exception();
        }
    }
    
    [Serializable]
    public struct DungeonData {
        [SerializeField]
        public string id;
        [SerializeField]
        public Vector2 coordinates;
        [SerializeField]
        public List<Vector2Int> entrances;
        [SerializeField]
        public string seed;
        [SerializeField]
        public int dungeonWidth; // Including only non-empty columns
        [SerializeField]
        public DungeonType typeOfDungeon;

        public DungeonData(string id, Vector2 coordinates, List<Vector2Int> entrances, string seed, int dungeonWidth, DungeonType typeOfDungeon) {
            this.id = id;
            this.coordinates = coordinates;
            this.entrances = entrances;
            this.seed = seed;
            this.dungeonWidth = dungeonWidth;
            this.typeOfDungeon = typeOfDungeon;
        }
    }
    
    public enum DungeonType {
        lostRoom,
        bossDungeon,
        questDungeon,
        regularDungeon
    }
}
