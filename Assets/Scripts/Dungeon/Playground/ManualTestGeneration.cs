using Dungeon;
using Global;
using UnityEngine;
using Manager.SubManager;
using UnityEngine.Serialization;

public class ManualTestGeneration : MonoBehaviour {

    [SerializeField] public int maxDungeons = 5;
    [SerializeField] public int dungeonChunkCount = 5;
    [SerializeField] public string seedExample = "5556444221";
    
    void Start() {
        GlobalVariables.environment = "DEV";
        Consts.Set("MaxDungeons", maxDungeons);
        Consts.Set("DungeonChunkCount", dungeonChunkCount);
        DungeonManager.Initialize(seedExample);
        DungeonManager.RenderDungeonsAll();
    }
}
