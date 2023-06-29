using System.Collections.Generic;
using Dungeon.Data;
using Dungeon.Generator;
using Global;
using Manager.SubManager;
using NUnit.Framework;
using UnityEngine;

namespace Dungeon.Playground {
    
    public class DungeonListTest {
        [Test]
        public void TestSimplePasses() {
            GlobalVariables.environment = "DEV";
            Consts.Set("MaxDungeons", 10);
            Consts.Set("DungeonChunkCount", 5);
            DungeonManager.Initialize("5556444221");
            DungeonManager.RenderDungeonsAll();
        }
    }
    
}

