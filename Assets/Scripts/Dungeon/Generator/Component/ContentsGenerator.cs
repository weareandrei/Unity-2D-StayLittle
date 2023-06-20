using System.Collections.Generic;
using Content;
using Dungeon.Model;

namespace Dungeon.Generator {
    public class ContentsGenerator {
        
        private static ChunkMap _chunkMap; // Will need to validate items per Chunk.
        private static RoomMap _roomMap;
        private static ExitMap _exitMap;

        private static ContentsMap _contentsMap;
        
        public static ContentsMap GenerateContents(RoomMap roomMap) {
            _contentsMap = new ContentsMap();
            // Insert a function to DisableWrongExits() here
            
            _roomMap = roomMap;

            _contentsMap.contentPointsAll = FindAllContentPoints();
            _contentsMap.contentPointsUsed = ContentPointSelector();
            
            _contentsMap.CreateContentDictionary();

            return new ContentsMap();
        }

        private static List<ContentPoint> FindAllContentPoints() {
            List<ContentPoint> contentPointsFound = new List<ContentPoint>();
            
            for (int y = 0; y < _roomMap.map.getYSize() - 1; y++) {
                for (int x = 0; x < _roomMap.map.getXSize() - 1; x++) {
                    string thisRoomID = _roomMap.map.GetCell(x, y);
                    if (thisRoomID != "") {
                        contentPointsFound.AddRange(FindThisRoomContentPoints(thisRoomID));
                    }
                }
            }

            return contentPointsFound;
        }

        private static List<ContentPoint> FindThisRoomContentPoints(string roomID) {
            Room thisRoom = RoomGenerator.FindRoomInstanceByID(roomID);
            return Room.GetRoomContentPoints(thisRoom);
        }

        private static List<ContentPoint> ContentPointSelector() {
            return _contentsMap.contentPointsAll;
        }
    }
}