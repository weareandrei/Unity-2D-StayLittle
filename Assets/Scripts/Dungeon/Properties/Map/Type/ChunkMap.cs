namespace Dungeon.Properties.Map.Type {
    public class ChunkMap : DungeonMap {
        public struct PossibleExit {
            public int x;
            public int y;
            public SidePosition position;
        }
        public enum SidePosition {
            Top, Bottom, Left, Right
        }
    }
    
    
}