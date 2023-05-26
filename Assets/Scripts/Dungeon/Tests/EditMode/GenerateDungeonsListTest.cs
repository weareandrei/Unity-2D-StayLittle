using Dungeon.Renderer;
using Dungeon.Properties;
using NUnit.Framework;
using DataPersistence;

namespace Dungeon.Tests.EditMode {
    public class GenerateDungeonsListTest
    {
        [Test]
        public void CorrectMapGenerated() {
            Managers.DungeonManager.Initialize("3962420980463");
            Managers.DungeonManager.RenderDungeonsAll();
        }
    }
}
