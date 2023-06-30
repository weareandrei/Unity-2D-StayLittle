namespace HoneyGrid2D {
    public class FlexGrid2DInt : FlexGrid2D<int> {
        
        public FlexGrid2DInt(int sizeX, int sizeY) : base(sizeX, sizeY, 0) {
            initialCellValue = 0;
        }
        
        public override object Clone() {
            FlexGrid2DInt clone = new FlexGrid2DInt(rows[0].cells.Count, rows.Count);
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