using Array2DEditor;
using System.Collections.Generic;
using UnityEngine;

using Dungeon.Properties;
using Dungeon.Properties.MapUtilities;

namespace Dungeon.Properties.MapUtilities {
    
    public class DungeonMap {
        
        public List<Column> columns;

        
        // Create 1 Chunk for the entrance to initialise the map
        public DungeonMap() {
            columns = new List<Column>();

            for (var x = 0; x < DungeonConsts.defaultChunkSize; x++) {
                Column newColumn = new Column();
                
                for (var y = 0; y < DungeonConsts.defaultChunkSize; y++) {
                    newColumn.column.Add("E");
                }
                
                columns.Add(newColumn);
            }
        }


        public void InsertRoom (int x, int y) {
            
        }
        

        public void ExpandDown (int count) {
            columns.ForEach( (column) => column.insertEmptyInFront(count) );
        }

        public void ExpandUp (int count) {
            columns.ForEach( (column) => column.insertEmptyInBack(count) );
        }

        public void ExpandRight(int count) {
            int rowCount = columns[0].column.Count; // Count number of rows in the column
            for (var i = 0; i < count; i++) {
                Column newColumn = new Column();
                for (var j = 0; j < rowCount; j++) {
                    newColumn.column.Add("E");
                }
                columns.Add(newColumn);
            }
        }

        public void DisplayMap () {
            int rowCount = columns[0].column.Count;
            int columnCount = columns.Count;

            for (var y = 0; y < rowCount; y++) {
                for (var x = columnCount; x < 0; x--) {
                    Debug.Log(columns[x].column[y]);
                }
            }
        }

        public string GetRoom(int x, int y) {
            return columns[x].column[y];
        }
        
        public struct ExitData {
            public int x;
            public int y;
            public ExitDirection exitDirection;
        };

        public enum ExitDirection {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}
