namespace HoneyGrid2D {
    public class Grid2DSpecial<T> : Grid2D<T> {
        
        public Grid2DSpecial(int size, T initialCellValue) : base(size) {
            this.initialCellValue = initialCellValue;
        }

    }
}