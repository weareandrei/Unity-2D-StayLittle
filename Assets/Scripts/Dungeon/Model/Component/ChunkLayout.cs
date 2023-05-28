using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Model {
    
    public class ChunkLayout : MonoBehaviour {
        // nothing - empty, R - room, E - entrance
        [SerializeField] public string ID;
        [SerializeField] public Grid2D rooms;
        [SerializeField] public ChunkType chunkType;

        public ChunkLayout() {
            rooms = new Grid2D(Consts.ChunkSize);
        }

        public ChunkLayout(int size) {
            rooms = new Grid2D(size);
        }
    }

    public enum ChunkType {
        Entrance,
        Default
    }
    
}