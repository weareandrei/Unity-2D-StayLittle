using Dungeon;
using Global;
using UnityEngine;
using Manager.SubManager;

public class ManualTestGeneration : MonoBehaviour
{
    void Start() {
        GlobalVariables.environment = "DEV";
        Consts.Set("MaxDungeons", 3);
        Consts.Set("DungeonChunkCount", 5);
        DungeonManager.Initialize("5556444221");
        DungeonManager.RenderDungeonsAll();
    }
}
