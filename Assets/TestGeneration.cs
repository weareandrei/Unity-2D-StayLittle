using DataPersistence;
using Dungeon.Generator;
using Dungeon.Properties;
using Dungeon.Renderer;
using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    void Start()
    {
        Debug.Log("GetDungeonDataByIndex");
        DungeonData dungeonData = DungeonList.GetDungeonDataByIndex(0);
        
        Debug.Log("LoadResourses");
        Generator.LoadResources();
        Debug.Log("GenerateDungeonBySeed");
        DungeonMapData dungeonMapData = Generator.GenerateDungeonBySeed(dungeonData.seed);
            
        Debug.Log("RenderDungeon");
        DungeonRenderer.RenderDungeon(dungeonData, dungeonMapData);
    }
}
