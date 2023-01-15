using System.Collections.Generic;

namespace Dungeon.Properties.MapUtilities {
    
    public class Column {
        
        public List<string> column = new List<string>(); // List containing room id's

        public void insertEmptyInFront(int count) {
            for (var i = 0; i < count; i++) {
                column.Add("E"); // E = Empty
            }
        }
        
        public void insertEmptyInBack(int count) {
            for (var i = 0; i < count; i++) {
                column.Insert(0,"E"); // E = Empty
            }
        }
        
    }
    
}