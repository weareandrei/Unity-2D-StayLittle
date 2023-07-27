namespace HoneyGrid2D {
    public class FlexGrid2DString : FlexGrid2D<string> {
        
        public FlexGrid2DString(int sizeX, int sizeY) : base(sizeX, sizeY, "") {
            initialCellValue = "";
        }
        
        public override object Clone() {
            FlexGrid2DString clone = new FlexGrid2DString(rows[0].cells.Count, rows.Count);
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