using Dungeon;
using NUnit.Framework;
using Dungeon.Generator;
using Dungeon.Model;
using Global;
using HoneyGrid2D;
using UnityEngine;

public class DungeonGenerationTests
{
    
    [SetUp]
    public void SetUp() {
        GlobalVariables.environment = "DEV";
        Consts.Set("DungeonChunkCount", 5);
        DungeonGenerator.LoadResources();
    }

    [Test]
    public void DungeonGeneratedCorrectly() {
        DungeonMapData dungeonMapData = DungeonGenerator.GenerateDungeonBySeed("3962420980463", new Vector2(-1, 0));

        CheckChunks(dungeonMapData.chunkMap.map);
        // CheckRooms(dungeonMapData.roomMap.map);
    }

    private void CheckChunks(FlexGrid2DString chunkMap) {
        Assert.AreEqual("2", chunkMap.GetCellActual(0, 0));
        Assert.AreEqual("2", chunkMap.GetCellActual(0, 1));
        Assert.AreEqual("2", chunkMap.GetCellActual(0, 2));
        Assert.AreEqual("8", chunkMap.GetCellActual(1, 2));
        Assert.AreEqual("2", chunkMap.GetCellActual(0, 3));
        Assert.AreEqual("5", chunkMap.GetCellActual(1, 3));
        Assert.AreEqual("2", chunkMap.GetCellActual(0, 4));
        Assert.AreEqual("5", chunkMap.GetCellActual(1, 4));

        // Test Entrance.
        Assert.AreEqual("1", chunkMap.GetCellActual(0, 5));
        Assert.AreEqual("8", chunkMap.GetCellActual(1, 5));
    }
    
    // private void CheckRooms(FlexGrid2DString roomMap) {
    //     // todo:  ZeroYOffset is 0. Is it supposed to be like that ?
    //     Assert.AreEqual("2", roomMap.GetCellActual(0, 0));
    //     Assert.AreEqual("2", roomMap.GetCellActual(0, 1));
    //     Assert.AreEqual("2", roomMap.GetCellActual(0, 2));
    //     Assert.AreEqual("8", roomMap.GetCellActual(1, 2));
    //     Assert.AreEqual("2", roomMap.GetCellActual(0, 3));
    //     Assert.AreEqual("5", roomMap.GetCellActual(1, 3));
    //     Assert.AreEqual("2", roomMap.GetCellActual(0, 4));
    //     Assert.AreEqual("5", roomMap.GetCellActual(1, 4));
    //
    //     // Test Entrance.
    //     Assert.AreEqual("1", roomMap.GetCellActual(0, 5));
    //     Assert.AreEqual("8", roomMap.GetCellActual(1, 5));
    // }
    
}