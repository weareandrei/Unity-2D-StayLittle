using System.Collections.Generic;
using System.Linq;
using Dungeon.Chunk;
using Dungeon.Generator.Stage;
using Dungeon.Properties.Map.Type;
using NUnit.Framework;
using UnityEngine;

namespace Dungeon.Tests.EditMode {
    public class ChunkGeneratorTest
    {
        private static List<ChunkLayout> _chunkLayoutsAvailable;
    
        [Test]
        public void CorrectChunksGenerated() {
            // todo: this test is not final yet
            LoadResources();
            ChunkGenerator.chunkLayoutsAvailable = _chunkLayoutsAvailable;
            ChunkMap chunkMap = ChunkGenerator.GenerateChunks();
        }
        
        public static void LoadResources() {
            const string layoutsPath = "Dungeon/ChunkLayout/Prefabs";
            GameObject[] layoutsPrefabs = Resources.LoadAll<GameObject>(layoutsPath);
            ChunkLayout[] chunkLayouts = layoutsPrefabs.Select(
                prefabObj => prefabObj.GetComponent<ChunkLayout>()
            ).ToArray();
            _chunkLayoutsAvailable = new List<ChunkLayout>(chunkLayouts);
        }
    
    }
}
