using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Model {
    public class Wall : MonoBehaviour {
        public Vector2Int coordinates; // Don't use (Used in WallManager to form WallGrid)

        public bool wallEnabled;
        
        public Wall() {
            wallEnabled = false;
        }
        
        public Wall(int x, int y) {
            coordinates = new Vector2Int(x, y);
            wallEnabled = false;
        }
        
    }
}