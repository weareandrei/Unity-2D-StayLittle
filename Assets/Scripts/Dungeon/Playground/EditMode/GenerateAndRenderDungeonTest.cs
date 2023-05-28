using Dungeon.Data;
using Dungeon.Model;
using Dungeon.Renderer;
using NUnit.Framework;

namespace Dungeon.Playground {
    
    public class GenerateAndRenderDungeonTest
    {
        [Test]
        public void TestSimplePasses() {
            
            // Old test. Might not work correctly.
            
            DungeonData dungeonData = DungeonList.GetDungeonDataByIndex(0);
                
            Generator.DungeonGenerator.LoadResources();
            DungeonMapData dungeonMapData = Generator.DungeonGenerator.GenerateDungeonBySeed(dungeonData.seed);
                
            DungeonRenderer.RenderDungeon(dungeonData, dungeonMapData);
        }
    }
    
}



