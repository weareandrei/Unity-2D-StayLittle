using UnityEngine;

namespace HoneyGrid2D {
    
    [System.Serializable]
    public class Grid2DBool : Grid2D<bool> {
        
        [SerializeField]
        public Grid2DBool(int size) : base(size) {
            initialCellValue = false;
        }
        
    }
}