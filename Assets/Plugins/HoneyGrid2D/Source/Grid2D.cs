using UnityEngine;
using System;

namespace HoneyGrid2D {
    [System.Serializable]
    public abstract class Grid2D<T> {
        
        [SerializeField]
        private Row<T>[] rows;

        protected T initialCellValue;
        
        [SerializeField]
        private Vector2Int gridSize;

        public int Size {
            get => gridSize.x;
            set => gridSize = Vector2Int.one * value;
        }
        
        public Grid2D(int size) {
            rows = new Row<T>[size];
            for (int y=0; y < size; y++) {
                rows[y] = new Row<T>(size, initialCellValue);
            }
            Size = size;
        }
        
        public T GetCell(int x, int y) {
            return rows[y].cells[x];
        }
        
        public void UpdateCell(int x, int y, T contents) {
            rows[y].cells[x] = contents;
        }
        
        public void LoopThroughCells(Func<int, int, LoopState> func)
        {
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    LoopState loopState =  func(x, y);
                    if (loopState == LoopState.Break)
                    {
                        return;
                    }
                }
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
    public enum LoopState {
        Continue,
        Break
    }
}