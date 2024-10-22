using System.Collections.Generic;
using Dungeon;
using Dungeon.Data;
using Dungeon.Generator;
using Global;
using UnityEngine;
using Manager.SubManager;

public class ManualTestSingleDungeon : MonoBehaviour
{
    void Start() {
        GlobalVariables.environment = "DEV";
        Consts.Set("MaxDungeons", 1);
        Consts.Set("DungeonChunkCount", 5);
        
        // DungeonManager.Initialize("5556444221");
        // DungeonGenerator.GenerateDungeonBySeed('5556444221');
        DungeonGenerator.LoadResources();
        DungeonData dungeonData = new DungeonData("3", new Vector2Int(0,256), new List<Vector2Int>(), "2549585970", 7, DungeonType.questDungeon);
        DungeonList.dungeons = new List<DungeonData>();
        DungeonList.dungeons.Add(dungeonData);
        DungeonManager.RenderDungeonsAll();
    }
}