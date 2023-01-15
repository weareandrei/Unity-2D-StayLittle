using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DungeonsData
{

    private static List<_dungeonData> dungeons = new List<_dungeonData>() {
        {new _dungeonData("a",100,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("b",200,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("c",300,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("d",400,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("e",500,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("f",600,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("g",700,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("h",800,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("i",900,"seed",_dungeonType.regularDungeon)},
        {new _dungeonData("j",1000,"seed",_dungeonType.regularDungeon)}
    }; // Assume its sorted already

    public static int countDungeons() {
        return dungeons.Count;
    }

    public static _dungeonData getDungeonDataByIndex(int index) {
        return dungeons[index];
    }

    public static void SaveDungeons() {

    }

    public static void LoadDungeons() {

    }

    public struct _dungeonData {
        public string id;
        public int depthLevel;
        public string seed;
        public _dungeonType typeOfDungeon;

        public _dungeonData(string id, int depthLevel, string seed, _dungeonType typeOfDungeon) {
            this.id = id;
            this.depthLevel = depthLevel;
            this.seed = seed;
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
