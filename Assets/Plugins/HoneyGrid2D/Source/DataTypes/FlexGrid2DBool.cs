namespace HoneyGrid2D {
    public class FlexGrid2DBool: FlexGrid2D<bool> {
        
        public FlexGrid2DBool(int sizeX, int sizeY) : base(sizeX, sizeY) {
            initialCellValue = false;
        }
        
        public FlexGrid2DBool(int size) : base(size) {
            initialCellValue = false;
        }

        public override object Clone() {
            FlexGrid2DBool clone = new FlexGrid2DBool(rows[0].cells.Count, rows.Count);
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