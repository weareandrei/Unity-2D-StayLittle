namespace Grid2DEditor
{
    [System.Serializable]
    public class Row {
        
        public string[] cells; // List containing room id's
        
        public Row(int size) {
            cells = new string[size];
            
            for (int x = 0; x < size; x++) {
                cells[x] = "";
            }
        }

    }
}
