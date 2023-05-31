using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Model {
    
    public class Chunk : MonoBehaviour {
        // nothing - empty, R - room, E - entrance
        [SerializeField] public string ID;
        [SerializeField] public Grid2DString rooms;
        [SerializeField] public ChunkType chunkType;

        public Chunk() {
            rooms = new Grid2DString(Consts.ChunkSize);
        }

        public Chunk(int size) {
            rooms = new Grid2DString(size);
        }
    }

    public enum ChunkType {
        Entrance,
        Default
    }
    
}