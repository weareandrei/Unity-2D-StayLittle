using System;
using System.Collections.Generic;
using System.Linq;

namespace HoneyGrid2D {
    public abstract class FlexGrid2D<T>  : ICloneable {
        protected List<FlexRow<T>> rows;
        public int zeroYOffset = 0;

        protected T initialCellValue;
        
        public FlexGrid2D(int sizeX, int sizeY, T initialCellValue) {
            this.initialCellValue = initialCellValue;
            rows = new List<FlexRow<T>>();

            for (int y = 0; y < sizeY; y++) {
                FlexRow<T> newRow = new FlexRow<T>(sizeX, initialCellValue);
                rows.Add(newRow);
            }
        }

        // This is an Abstract GetCel.. We consider offset here.
        public T GetCell(int x, int y) {
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
        
        // This is an Actual GetCell.. We do not consider offset here.
        public T GetCellActual(int x, int y) {
            if (x < 0 || x > rows[0].cells.Count-1) {
                throw new IndexOutOfRangeException("X out of range");
            }

            if (y < 0 || y > rows.Count-1) {
                throw new IndexOutOfRangeException("X out of range");
            }

            return rows[y].cells[x];
        }

        public void UpdateCell(int x, int y, T contents) {
            rows[y + zeroYOffset].cells[x] = contents;
        }
        public void LoopThroughCells(Func<int, int, LoopState> func)
        {
            for (int y = 0; y < getYSize(); y++) {
                for (int x = 0; x < getXSize(); x++) {
                    LoopState loopState =  func(x, y);
                    if (loopState == LoopState.Break)
                    {
                        return;
                    }
                }
            }
        }

        public void ExpandRight (int count = 1) {
            rows.ForEach( (row) =>  row.InsertCellsRight(count) );
        }

        public void ExpandVertical(int direction, int count = 1) {
            //direction can be 1 or -1 (up, down)
            int gridWidth = rows[0].cells.Count; // Count number of cells in the row (x length of the grid)

            for (var i = 0; i < count; i++) {
                FlexRow<T> newRow = new FlexRow<T>(gridWidth, initialCellValue);
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

        // public void DisplayGrid () {
        //     Debug.Log("Grid's current state: ");
        //
        //     int columnSize = rows[0].cells.Count;
        //     int rowSize = rows.Count;
        //     
        //     Debug.Log("-- Rows: " + rowSize);
        //     Debug.Log("-- Columns: " + columnSize);
        //
        //     for (var y = rowSize-1; y >= 0; y--) {
        //         string rowStr = "";
        //         for (var x = 0; x <= columnSize-1; x++) {
        //             string appendWithCell = ConvertCellToReadable(rows[y].cells[x]);
        //             rowStr += appendWithCell;
        //             rowStr += " ";
        //         }
        //         Debug.Log("[" + rowStr + "]");
        //     }
        // }
        //
        // private T ConvertCellToReadable(T cellContents) {
        //     if (cellContents == "") {
        //         return "XX";
        //     }
        //
        //     if (cellContents.Length == 1) {
        //         cellContents = "0" + cellContents;
        //     }
        //
        //     return cellContents;
        // }
        
        public void RemoveEmptyRowsAndColumns() {
            RemoveEmptyRows();
            RemoveEmptyColumns();
        }

        private void RemoveEmptyRows() {
            for (int rowIndex = rows.Count - 1; rowIndex >= 0; rowIndex--) {
                if (IsRowEmpty(rowIndex)) {
                    rows.RemoveAt(rowIndex);
                    if (rowIndex == zeroYOffset - 1) {
                        zeroYOffset--;
                    }
                }
            }
        }

        private bool IsRowEmpty(int rowIndex)
        {
            FlexRow<T> row = rows[rowIndex];
            return row.cells.All(cell => cell.Equals(initialCellValue));
        }

        private void RemoveEmptyColumns() {
            if (rows.Count > 0) {
                int maxColumnIndex = rows[0].cells.Count - 1;

                for (int columnIndex = maxColumnIndex; columnIndex >= 0; columnIndex--) {
                    if (IsColumnEmpty(columnIndex)) {
                        RemoveColumn(columnIndex);
                    } else {
                        break;
                    }
                }
            }
        }

        private bool IsColumnEmpty(int columnIndex) {
            return rows.All(row => row.cells[columnIndex].Equals(initialCellValue));
        }

        private void RemoveColumn(int columnIndex) {
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++) {
                rows[rowIndex].cells.RemoveAt(columnIndex);
            }
        }
        public abstract object Clone();
    }
}