namespace HoneyGrid2D {
    public class FlexGrid2DFloat: FlexGrid2D<float> {
        
        public FlexGrid2DFloat(int sizeX, int sizeY) : base(sizeX, sizeY) {
            initialCellValue = 0f;
        }
        
        public FlexGrid2DFloat(int size) : base(size) {
            initialCellValue = 0f;
        }

        public override object Clone() {
            FlexGrid2DFloat clone = new FlexGrid2DFloat(rows[0].cells.Count, rows.Count);
            for (int y = 0; y < rows.Count; y++)
            {
                for (int x = 0; x < rows[y].cells.Count; x++)
                {
                    clone.UpdateCell(x, y, rows[y].cells[x]);
                }
            }
            return clone;
        }
    }
}