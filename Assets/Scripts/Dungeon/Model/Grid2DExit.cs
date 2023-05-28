using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Model {
    public class Grid2DExit {
        private List<ExitRow> rows;
        
        public Grid2DExit(int sizeX, int sizeY) {
            rows = new List<ExitRow>();

            for (int y = 0; y < sizeY; y++) {
                ExitRow newRow = new ExitRow(sizeX);
                rows.Add(newRow);
            }
        }
        
        public Exit.ExitMapCell GetCellActual(int x, int y) {
            if (x < 0 || x > rows[0].cells.Count-1) {
                throw new IndexOutOfRangeException("X out of range");
            }

            if (y < 0 || y > rows.Count-1) {
                throw new IndexOutOfRangeException("X out of range");
            }

            return rows[y].cells[x];
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