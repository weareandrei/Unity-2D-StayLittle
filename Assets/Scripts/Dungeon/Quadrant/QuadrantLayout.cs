using Array2DEditor;
using UnityEngine;

namespace Dungeon.Quadrant {
    
    public class QuadrantLayout : MonoBehaviour {
        // 0 - empty, 1 - room, 2 - entrance
        [SerializeField] public Array2DInt rooms;
        [SerializeField] public QuadrantType quadrantType; 
    }
    
    public enum QuadrantType {
        Entrance,
        Long,
        FullSize
    }
    
}