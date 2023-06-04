using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Model {
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
        
        public struct ExitMapCell : ICloneable {
            public List<SidePosition> exits;
            public SidePosition mainExitDirection;

            public ExitMapCell(List<SidePosition> exits, SidePosition mainExitDirection) {
                this.exits = exits;
                this.mainExitDirection = mainExitDirection;
            }

            public object Clone() {
                // Create a new instance of ExitMapCell with cloned values
                return new ExitMapCell(
                    new List<SidePosition>(exits), // Perform a shallow copy of the List
                    mainExitDirection
                );
            }
        }

        
        public enum SidePosition {
            None, Top, Bottom, Left, Right
        }
    }
}