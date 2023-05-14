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
            
        }
    }
}