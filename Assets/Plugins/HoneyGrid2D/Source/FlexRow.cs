using System.Collections.Generic;

namespace HoneyGrid2D
{
    [System.Serializable]
    public class FlexRow<T> {
        
        public readonly List<T> cells = new List<T>(); // List containing room id's
        private T initialValue;
        
        public FlexRow(int size, T initialValue) {
            this.initialValue = initialValue; 
            for (int i = 0; i < size; i++) {
                cells.Add(initialValue);
            }
        }
        
        public void InsertCellsRight(int count) {
            for (var x = 0; x < count; x++) {
                cells.Add(initialValue);
            }
        }

    }
}