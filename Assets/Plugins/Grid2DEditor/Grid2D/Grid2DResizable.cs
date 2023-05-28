using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid2DEditor {
    
    public class Grid2DResizable : ICloneable {
        
        private List<RowResizable> rows;
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

        public int zeroYOffset = 0; 
        
        public Grid2DResizable(int sizeX, int sizeY) {
            rows = new List<RowResizable>();

            for (int y = 0; y < sizeY; y++) {
                RowResizable newRow = new RowResizable(sizeX);
                rows.Add(newRow);
            }
        }
        
        public Grid2DResizable(int size) {
            rows = new List<RowResizable>();

            for (int y = 0; y < size; y++) {
                RowResizable newRow = new RowResizable(size);
                rows.Add(newRow);
            }
        }
        
        // This is an Abstract GetCel.. We consider offset here.
        public string GetCell(int x, int y) {
            if (x < 0) {
                throw new IndexOutOfRangeException("X out of range");
            }

            if (x >= rows[0].cells.Count) {
                ExpandRight(1);
            }

            if (y + zeroYOffset < 0) {
                ExpandVertical(-1);
            }
            
            if (y + zeroYOffset >= rows.Count) {
                ExpandVertical(1);
            }
            
            return rows[y + zeroYOffset].cells[x];
        }
        
        // This is an Actual GetCel.. We do not consider offset here.
        public string GetCellActual(int x, int y) {
            if (x < 0 || x > rows[0].cells.Count-1) {
                throw new IndexOutOfRangeException("X out of range");
            }

            if (y < 0 || y > rows.Count-1) {
                throw new IndexOutOfRangeException("X out of range");
            }

            return rows[y].cells[x];
        }

        public void UpdateCell(int x, int y, string contents) {
            rows[y + zeroYOffset].cells[x] = contents;
        }

        public void ExpandRight (int count = 1) {
            rows.ForEach((row) =>  row.InsertCellsRight(count) );
        }

        public void ExpandVertical(int direction, int count = 1) {
            //direction can be 1 or -1 (up, down)
            int gridWidth = rows[0].cells.Count; // Count number of cells in the row (x length of the grid)

            for (var i = 0; i < count; i++) {
                RowResizable newRow = new RowResizable(gridWidth);
                switch (direction) {
                    case 1:
                        rows.Add(newRow);
                        break;
                    case -1:
                        rows.Insert(0, newRow);
                        zeroYOffset++;
                        break;
                }
            }
        }

        public int getYSize() {
            return rows.Count;
        }
        
        public int getXSize() {
            return rows[0].cells.Count;
        }

        public void DisplayGrid () {
            Debug.Log("Grid's current state: ");

            int columnSize = rows[0].cells.Count;
            int rowSize = rows.Count;
            
            Debug.Log("-- Rows: " + rowSize);
            Debug.Log("-- Columns: " + columnSize);
        
            for (var y = rowSize-1; y >= 0; y--) {
                string rowStr = "";
                for (var x = 0; x <= columnSize-1; x++) {
                    string appendWithCell = ConvertCellToReadable(rows[y].cells[x]);
                    rowStr += appendWithCell;
                    rowStr += " ";
                }
                Debug.Log("[" + rowStr + "]");
            }
        }
        
        private string ConvertCellToReadable(string cellContents) {
            if (cellContents == "") {
                return "XX";
            }

            if (cellContents.Length == 1) {
                cellContents = "0" + cellContents;
            }

            return cellContents;
        }
        
        public object Clone()
        {
            Grid2DResizable clone = new Grid2DResizable(rows[0].cells.Count, rows.Count);
            for (int y = 0; y < rows.Count; y++)
            {
                for (int x = 0; x < rows[y].cells.Count; x++)
                {
                    clone.UpdateCell(x, y, rows[y].cells[x]);
                }
            }
            return clone;
        }

    }
}
