using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dungeon.Properties.Map.Util {
    public class Row {
        
        public readonly List<string> cells = new List<string>(); // List containing room id's

        // public void InsertEmptyInFront(int count) {
        //     for (var i = 0; i < count; i++) {
        //         column.Add("E"); // E = Empty
        //     }
        // }

        public Row(int size) {
            for (int i = 0; i < size; i++) {
                cells.Add("");
            }
        }
        
        public void InsertCellsRight(int count) {
            for (var x = 0; x < count; x++) {
                cells.Add("");
            }
        }
        
    }
}
