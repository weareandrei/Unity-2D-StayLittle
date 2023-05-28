using System.Collections.Generic;
using System.Xml.Serialization;

namespace Grid2DEditor {
    public class RowResizable {
        
        public readonly List<string> cells = new List<string>(); // List containing room id's

        public RowResizable(int size) {
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
