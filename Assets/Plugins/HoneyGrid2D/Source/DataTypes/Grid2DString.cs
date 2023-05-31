using UnityEngine;

namespace HoneyGrid2D {
    
    [System.Serializable]
    public class Grid2DString : Grid2D<string> {
        
        [SerializeField]
        public Grid2DString(int size) : base(size) {
            initialCellValue = "";
        }

    }
}