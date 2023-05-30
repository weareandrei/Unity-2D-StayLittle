namespace HoneyGrid2D {
    public class FlexGrid2DSpecial<T> : FlexGrid2D<T> {
        
        public FlexGrid2DSpecial(int sizeX, int sizeY, T initialCellValue) : base(sizeX, sizeY) {
            this.initialCellValue = initialCellValue;
        }
        
        public FlexGrid2DSpecial(int size, T initialCellValue) : base(size) {
            this.initialCellValue = initialCellValue;
        }

        public override object Clone() {
            FlexGrid2DSpecial<T> clone = new FlexGrid2DSpecial<T>(rows[0].cells.Count, rows.Count, initialCellValue);
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