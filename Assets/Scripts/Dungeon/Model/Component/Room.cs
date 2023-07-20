using System.Collections.Generic;
using Content;
using Dungeon.Generator;
using UnityEngine;
using HoneyGrid2D;

namespace Dungeon.Model {

    public class Room : MonoBehaviour {
        public string roomID;
        public string roomInstanceID;
        public RoomType type;
        
        // 0 - empty, 1 - occupied, 2 - exit, 3 - not strict exit (on this side any cell)
        [SerializeField] public Grid2DString roomLayout; 
        // Must be roomSize + 1.
        //  edge is only for entrance position and direction indication.
        // Because room layout must be much more precise than the chunkLayout

        public Room() {
            roomLayout = new Grid2DString(Consts.Get<int>("RoomSize")+2);
        }

        public Room(int size) {
            roomLayout = new Grid2DString(size+2);
        }
        
        
        public struct LinkedRoomsAddresses {
            public int top;
            public int bottom;
            public int left;
            public int right;
        };
        
        public List<GameObject> GetContentPoints() {
            // Find the "ContentPoints" GameObject in this Room Instance
            Transform contentPointsParent = transform.Find("ContentPoints");
            List<GameObject> contentPoints = new List<GameObject>();
    
            // Then find it's children "ContentPoint"
            if (contentPointsParent != null) {
                foreach (Transform childTransform in contentPointsParent) {
                    // ContentPoint contentPointData = childTransform.GetComponent<ContentPoint>();
                    // contentPoints.Add(childTransform.GetComponent<ContentPoint>());
                    contentPoints.Add(childTransform.gameObject);
                }
            } else {
                Debug.LogWarning("Unable to find 'ContentPoints' child GameObject.");
            }

            return contentPoints;
        }
        
        // public static List<ContentPoint> GetRoomContentPoints(Room room) {
        //     return room.GetContentPoints();
        // }


    }

    public enum RoomType {
        Normal,
        EntranceRight,
        EntranceLeft
    }
}
