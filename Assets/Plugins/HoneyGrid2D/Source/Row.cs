namespace HoneyGrid2D
{
    [System.Serializable]
    public class Row<T> {
        
        public T[] cells; // List containing room id's
        
        public Row(int size, T initialValue) {
            cells = new T[size];
            
            for (int x = 0; x < size; x++) {
                cells[x] = initialValue;
            }
        }

    }
}