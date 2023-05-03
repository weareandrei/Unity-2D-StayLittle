using DataPersistence;
using Dungeon.Generator;
using Dungeon.Properties;
using Dungeon.Renderer;
using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    void Start()
    {
        DungeonData dungeonData = DungeonList.GetDungeonDataByIndex(0);
        
        Generator.LoadResources();
        DungeonMapData dungeonMapData = Generator.GenerateDungeonBySeed(dungeonData.seed);
            
        DungeonRenderer.RenderDungeon(dungeonData, dungeonMapData);
    }
}
