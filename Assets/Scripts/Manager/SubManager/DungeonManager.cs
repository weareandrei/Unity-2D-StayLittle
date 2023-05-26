using DataPersistence;
using Dungeon.Generator;
using Dungeon.Renderer;
using UnityEngine;

namespace Managers {
    public static class DungeonManager {
        
        public static void Initialize(string seed = "") {
            GenerateDungeonList(seed);
        }
        
        private static void GenerateDungeonList(string seed = "") {
            if (string.IsNullOrEmpty(seed)) {
                seed = Util.Random.GenerateSeed();
            }
            // use the seed to generate the dungeon list
            DungeonList._dungeons = Dungeon.Generator.DungeonListGenerator.Generate(10, seed);
        }

        public static void RenderDungeonsAll() {
            foreach (DungeonData dungeonData in DungeonList._dungeons) {
                DungeonRenderer.RenderDungeon(
                    dungeonData, 
                    Generator.GenerateDungeonBySeed(dungeonData.seed)
                );
            }
        }
    }
}