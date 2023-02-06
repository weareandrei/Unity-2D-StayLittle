using UnityEngine;
using Grid2DEditor;

namespace Dungeon.Room {

    public class RoomInstance : MonoBehaviour {
        public int roomID;
        public string roomInstanceID;
        public RoomType type;

        [SerializeField] public Grid2D roomLayout; // 0 - empty, 1 - occupied, 2 - entrance
        // Must be roomSize + 1.
        //  edge is only for entrance position and direction indication.
        
        public struct LinkedRoomsAddresses {
            public int top;
            public int bottom;
            public int left;
            public int right;
        };
    }

    public enum RoomType {
        Entrance
    }
}
