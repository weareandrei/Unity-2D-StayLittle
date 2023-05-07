using UnityEngine;

namespace Grid2DEditor 
{
    [System.Serializable]
    public class Grid2D {
        
        [SerializeField]
        private Row[] rows;
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
        
        [SerializeField]
        private Vector2Int gridSize;

        public int Size {
            get => gridSize.x;
            set => gridSize = Vector2Int.one * value;
        }
        
        public Grid2D(int size) {
            rows = new Row[size];
            for (int y=0; y < size; y++) {
                rows[y] = new Row(size);
            }
            Size = size;
        }
        

        public string GetCell(int x, int y) {
            return rows[y].cells[x];
        }
        
        public void UpdateCell(int x, int y, string contents) {
            rows[y].cells[x] = contents;
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
