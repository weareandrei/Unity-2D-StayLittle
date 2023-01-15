using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

namespace Dungeon.Room {

    public class RoomInstance : MonoBehaviour {
        public int roomID;
        public string roomInstanceID;
        public RoomType type;

        [SerializeField] public Array2DInt roomLayout; // 0 - empty, 1 - occupied, 2 - entrance
        // Must be roomSize + 1.
        //  edge is only for entrance position and direction indication.
                                                        
        // this is for room contents (1 - need to be empty) 0 by default.
        [SerializeField] public Array2DBool requirementsTop;
        [SerializeField] public Array2DBool requirementsBottom;
        [SerializeField] public Array2DBool requirementsLeft;
        [SerializeField] public Array2DBool requirementsRight;

        public LinkedRoomsAddresses linkedRooms;

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
