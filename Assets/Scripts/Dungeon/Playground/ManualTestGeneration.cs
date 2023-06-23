using Dungeon;
using Global;
using UnityEngine;
using Manager.SubManager;

public class ManualTestGeneration : MonoBehaviour
{
    void Start() {
        GlobalVariables.environment = "DEV";
        Consts.Set("MaxDungeons", 1);
        Consts.Set("DungeonChunkCount", 5);
        DungeonManager.Initialize("333333333");
        DungeonManager.RenderDungeonsAll();
    }
}
