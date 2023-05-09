using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Properties.Map.Util {
    public class Grid2DExit {
        private List<ExitRow> rows;
        
        public Grid2DExit(int sizeX, int sizeY) {
            rows = new List<ExitRow>();

            for (int y = 0; y < sizeY; y++) {
                ExitRow newRow = new ExitRow(sizeX);
                rows.Add(newRow);
            }
        }
        
        public void UpdateCell(Vector2Int cellCoordintates, Exit.ExitMapCell value) {
            rows[cellCoordintates.y].cells[cellCoordintates.x] = value;
        }
    }

    public class ExitRow {
        public List<Exit.ExitMapCell> cells = new List<Exit.ExitMapCell>();

        public ExitRow(int size) {
            for (int i = 0; i < size; i++) {
                cells.Add(new Exit.ExitMapCell());
            }
        }
    }
    
}