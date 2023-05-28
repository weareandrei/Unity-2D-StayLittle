using Dungeon.Generator;
using UnityEngine;
using Grid2DEditor;

namespace Dungeon.Model {

    public class RoomInstance : MonoBehaviour {
        public string roomID;
        public string roomInstanceID;
        public RoomType type;

        // 0 - empty, 1 - occupied, 2 - exit, 3 - not strict exit (on this side any cell)
        [SerializeField] public Grid2D roomLayout; 
        // Must be roomSize + 1.
        //  edge is only for entrance position and direction indication.
        // Because room layout must be much more precise than the chunkLayout

        public RoomInstance() {
            roomLayout = new Grid2D(Consts.RoomSize+2);
        }

        public RoomInstance(int size) {
            roomLayout = new Grid2D(size+2);
        }
        
        
        public struct LinkedRoomsAddresses {
            public int top;
            public int bottom;
            public int left;
            public int right;
        };
    }

    public enum RoomType {
        Normal,
        Entrance
    }
}
