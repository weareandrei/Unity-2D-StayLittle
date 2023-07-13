using Dungeon;
using Dungeon.Data;
using Dungeon.Generator;
using Dungeon.Renderer;
using Util;

namespace Manager.SubManager {
    public static class DungeonManager {
        
        public static void Initialize(string seed = "") {
            GenerateDungeonList(seed);
        }
        
        private static void GenerateDungeonList(string seed = "") {
            if (string.IsNullOrEmpty(seed)) {
                seed = Random.GenerateSeed();
            }
            // use the seed to generate the dungeon list
            DungeonList._dungeons = DungeonListGenerator.Generate(Consts.Get<int>("MaxDungeons"), seed);
        }

        public static void RenderDungeonsAll() {
            foreach (DungeonData dungeonData in DungeonList._dungeons) {
                DungeonRenderer renderer = new DungeonRenderer();
                renderer.RenderDungeon(
                    dungeonData, 
                    DungeonGenerator.GenerateDungeonBySeed(dungeonData.seed, dungeonData.coordinates)
                );
            }
        }
    }
}