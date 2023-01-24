using Array2DEditor;
using UnityEngine;

namespace Dungeon.Chunk {
    
    public class ChunkLayout : MonoBehaviour {
        // 0 - empty, 1 - room, 2 - entrance
        [SerializeField] public Array2DInt rooms;
        [SerializeField] public ChunkType quadrantType; 
        
        public ChunkLayout(int size) {
            
        }
    }

    public enum ChunkType {
        Entrance
    }
    
}