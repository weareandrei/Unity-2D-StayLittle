namespace Dungeon.Properties.Map.Type {
    public class ChunkMap : DungeonMap {
        public struct PossibleExit {
            public int x;
            public int y;
            public SidePosition position;

            public PossibleExit(int x, int y, SidePosition pos) {
                this.x = x;
                this.y = y;
                position = pos;
            }
        }
        public enum SidePosition {
            Top, Bottom, Left, Right
        }
    }
    
}