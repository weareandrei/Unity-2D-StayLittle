using Dungeon.Data;
using Dungeon.Model;
using Dungeon.Renderer;
using Global;
using Manager.SubManager;
using NUnit.Framework;
using UnityEngine;

namespace Dungeon.Playground {
    
    public class GenerateAndRenderDungeonTest
    {
        [Test]
        public void TestSimplePasses() {
            GlobalVariables.environment = "DEV";
            Consts.Set("MaxDungeons", 1);
            Consts.Set("DungeonChunkCount", 5);
            DungeonManager.Initialize("5556444221");
            DungeonManager.RenderDungeonsAll();
        }
    }
    
}



