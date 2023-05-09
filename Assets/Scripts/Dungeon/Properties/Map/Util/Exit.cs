using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Properties.Map.Util {
    public class Exit {
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
        
        public struct ExitMapCell {
            public List<SidePosition> exits;
            public SidePosition mainExitDirection;

            public ExitMapCell(List<SidePosition> exits, SidePosition mainExitDirection) {
                this.exits = exits;
                this.mainExitDirection = mainExitDirection;
            }
        }
        
        public enum SidePosition {
            None, Top, Bottom, Left, Right
        }
    }
}