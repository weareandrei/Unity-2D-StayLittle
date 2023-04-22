using Dungeon.Renderer;
using Dungeon.Properties;
using NUnit.Framework;
using DataPersistence;

namespace Dungeon.Tests.EditMode {
    public class GeneratorTest
    {
        [Test]
        public void CorrectMapGenerated() {
            DungeonData dungeonData = DungeonList.GetDungeonDataByIndex(0);
            
            Generator.Generator.LoadResources();
            DungeonMapData dungeonMapData = Generator.Generator.GenerateDungeonBySeed(dungeonData.seed);
            
            DungeonRenderer.RenderDungeon(dungeonData, dungeonMapData);
        }
        
    }
}
