using Manager.SubManager;
using NUnit.Framework;

namespace Dungeon.Playground {
    
    public class DungeonListTest {
        [Test]
        public void TestSimplePasses() {
            DungeonManager.Initialize("3962420980463");
            DungeonManager.RenderDungeonsAll();
        }
    }
    
}

