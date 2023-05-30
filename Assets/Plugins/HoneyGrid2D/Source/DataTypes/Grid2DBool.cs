namespace HoneyGrid2D {
    public class Grid2DBool : Grid2D<bool> {
        
        public Grid2DBool(int size) : base(size) {
            initialCellValue = false;
        }
        
    }
}