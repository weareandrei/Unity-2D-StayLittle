using UnityEditor;

namespace HoneyGrid2D {
    
    [CustomPropertyDrawer(typeof(Grid2DString))]
    public class Grid2DBoolDrawer : Grid2DDrawer<bool> {
        // Customize these methods based on your desired cell size and appearance
        protected override float GetCellWidth() {
            return 20f;
        }

        protected override float GetCellHeight() {
            return 20f;
        }
    }
}