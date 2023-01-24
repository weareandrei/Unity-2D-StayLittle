using System.Collections.Generic;
using Dungeon.Generator;
using UnityEngine;

namespace Dungeon.Properties.Map.Util {
    
    public class Grid2DResizable {
        
        private List<Row> rows;
        // [  y's
        // [1,1,1,1,1,1,1,1],   -> x's'
        // [1,1,1,1,1,1,1,1],
        // [1,1,1,1,1,1,1,1],
        // [1,1,1,1,1,1,1,1],
        // [1,1,1,1,1,1,1,1],
        // [1,1,1,1,1,1,1,1],
        // [1,1,1,1,1,1,1,1],
        // [1,1,1,1,1,1,1,1]
        // ]
        // To view properly - mirror in y direction.
        
        public Grid2DResizable(int size) {
            rows = new List<Row>();

            for (int y = 0; y < size; y++) {
                Row newRow = new Row();
                
                for (int x = 0; x < size; x++) {
                    newRow.cells.Add("");
                }
                
                rows.Add(newRow);
            }
        }
        
        

        public string GetCell(int x, int y) {
            return rows[x].cells[y];
        }
        
        public void UpdateCell(int x, int y, string contents) {
            rows[x].cells[y] = contents;
        }

        public void ExpandRight (int count) {
            rows.ForEach((row) => 
                row.InsertCellsRight(count) );
        }

        public void ExpandDown(int direction) { //direction can be 1 or -1 (up, down)
            int gridWidth = rows[0].cells.Count; // Count number of cells in the row (x length of the grid)
            Row newRow = new Row();
            for (var x = 0; x < gridWidth; x++) {
                newRow.cells.Add("");
            }

            switch (direction) {
                case 1:
                    rows.Add(newRow);
                    break;
                case -1:
                    rows.Insert(0, newRow);
                    break;
            }
        }

        // public void DisplayGrid () {
        //     int rowCount = rows[0].column.Count;
        //     int columnCount = rows.Count;
        //
        //     for (var y = 0; y < rowCount; y++) {
        //         for (var x = columnCount; x < 0; x--) {
        //             Debug.Log(rows[x].column[y]);
        //         }
        //     }
        // }
    }
}
