using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Chunk {
    
    public class ChunkLayout : MonoBehaviour {
        // 0 - empty, R - room, E - entrance
        [SerializeField] public string ID;
        [SerializeField] public Grid2D rooms;
        [SerializeField] public ChunkType quadrantType; 
        
        public ChunkLayout(int size) {
            rooms = new Grid2D(size);
        }
    }

    public enum ChunkType {
        Entrance
    }
    
}