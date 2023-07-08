using System;
using System.Collections.Generic;
using Content;
using Dungeon.Model;
using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Generator {
    public class ContentsGenerator {
        
        private static ChunkMap _chunkMap; // Will need to validate items per Chunk.
        private static RoomMap _roomMap;
        private static ExitMap _exitMap;

        private static ContentsMap _contentsMap;
        
        public static ContentsMap GenerateContents(RoomMap roomMap) {
            _roomMap = roomMap;
            _contentsMap = new ContentsMap();
            
            _contentsMap.map = new FlexGrid2DSpecial<RoomContents>(
                _roomMap.map.getXSize(),
                _roomMap.map.getYSize(),
                new RoomContents()
            );

            EnableWalls();
            
            PrebuildContentsMap();
            
            // _contentsMap.contentPointsAll = FindAllContentPoints(); // todo: maybe CollectibleContentPoints only ?
            // _contentsMap.contentPointsUsed = ContentPointSelector();

            return _contentsMap;
        }

        private static void EnableWalls() {
            for (int y = 0; y < _roomMap.map.getYSize(); y++) {
                for (int x = 0; x < _roomMap.map.getXSize(); x++) {
                    _contentsMap.map.GetCellActual(x, y).walls = EnableThisRoomWalls(new Vector2Int(x, y));
                }
            }
        }

        private static FlexGrid2DBool EnableThisRoomWalls(Vector2Int roomCoordinates) {
            FlexGrid2DBool wallsMap = new FlexGrid2DBool(Consts.Get<int>("RoomSize") + 2,
                Consts.Get<int>("RoomSize") + 2, true);

            try {
                string roomID = _roomMap.map.GetCellActual(roomCoordinates.x, roomCoordinates.y);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                updateWallMap_Top(roomCoordinates, room, ref wallsMap);
                updateWallMap_Bottom(roomCoordinates, room, ref wallsMap);
                updateWallMap_Left(roomCoordinates, room, ref wallsMap);
                updateWallMap_Right(roomCoordinates, room, ref wallsMap);
                
                return wallsMap;
            } catch (Exception e) {
                return wallsMap;
            }
        }
        
        private static void updateWallMap_Top(Vector2Int roomCoord, Room thisRoom, ref FlexGrid2DBool wallsMap) {
            try {
                string roomID = _roomMap.map.GetCellActual(roomCoord.x, roomCoord.y + 1);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);

                int y = Consts.Get<int>("RoomSize") + 1;
                for (int x = 0; x < Consts.Get<int>("RoomSize")+1; x++) {
                    string thisCellContents = thisRoom.roomLayout.GetCell(x, 0);
                    string neighbourCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents == "2" && neighbourCellContents == "2") {
                        wallsMap.UpdateCell(x, 0, false);
                    }
                }
            } catch (Exception e) { }
        }
        
        private static void updateWallMap_Bottom(Vector2Int roomCoord, Room thisRoom, ref FlexGrid2DBool wallsMap) {
            try {
                string roomID = _roomMap.map.GetCellActual(roomCoord.x, roomCoord.y - 1);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int y = 0;
                for (int x = 0; x < Consts.Get<int>("RoomSize")+1; x++) {
                    string thisCellContents = thisRoom.roomLayout.GetCell(x, Consts.Get<int>("RoomSize") + 1);
                    string neighbourCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents == "2" && neighbourCellContents == "2") {
                        wallsMap.UpdateCell(x, Consts.Get<int>("RoomSize")+1, false);
                    }
                }
            } catch (Exception e) { }
        }
        
        private static void updateWallMap_Left(Vector2Int roomCoord, Room thisRoom, ref FlexGrid2DBool wallsMap) {
            try {
                string roomID = _roomMap.map.GetCellActual(roomCoord.x + 1, roomCoord.y);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);

                int x = Consts.Get<int>("RoomSize") + 1;
                for (int y = 0; y < Consts.Get<int>("RoomSize")+1; y++) {
                    string thisCellContents = thisRoom.roomLayout.GetCell(0, y);
                    string neighbourCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents == "2" && neighbourCellContents == "2") {
                        wallsMap.UpdateCell(0, y, false);
                    }
                }
            } catch (Exception e) { }
        }
        
        private static void updateWallMap_Right(Vector2Int roomCoord, Room thisRoom, ref FlexGrid2DBool wallsMap) {
            try {
                string roomID = _roomMap.map.GetCellActual(roomCoord.x - 1, roomCoord.y);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int x = 0;
                for (int y = 0; y < Consts.Get<int>("RoomSize")+1; y++) {
                    string thisCellContents = thisRoom.roomLayout.GetCell(Consts.Get<int>("RoomSize")+1, y);
                    string neighbourCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents == "2" && neighbourCellContents == "2") {
                        wallsMap.UpdateCell(Consts.Get<int>("RoomSize")+1, y, false);
                    }
                }
            } catch (Exception e) { }
        }

        private static void PrebuildContentsMap() {
            for (int y = 0; y < _roomMap.map.getYSize(); y++) {
                for (int x = 0; x < _roomMap.map.getXSize(); x++) {
                    string thisRoomID = _roomMap.map.GetCellActual(x, y);
                    if (thisRoomID != "") {
                        List<ContentPoint> contentPoints =
                            RoomGenerator.FindRoomInstanceByID(thisRoomID).GetContentPoints();
                        List<ContentPayload> payloads = new List<ContentPayload>();
                        
                        foreach (ContentPoint contentPoint in contentPoints) {
                            ContentPayload payload = new ContentPayload(contentPoint.type);
                            payloads.Add(payload);
                        }

                        RoomContents roomContents = _contentsMap.map.GetCellActual(x, y);
                        roomContents.payloads = payloads;
                        
                        _contentsMap.map.UpdateCell(x, y, roomContents);
                    }
                }
            }
        }

        // private static List<ContentPointData> FindAllContentPoints() {
        //     List<ContentPointData> contentPointsFound = new List<ContentPointData>();
        //     
        //     for (int y = 0; y < _roomMap.map.getYSize() - 1; y++) {
        //         for (int x = 0; x < _roomMap.map.getXSize() - 1; x++) {
        //             string thisRoomID = _roomMap.map.GetCellActual(x, y);
        //             if (thisRoomID != "") {
        //                 CloneableList<ContentPointData> contentPoints = FindThisRoomContentPoints(thisRoomID);
        //                 contentPointsFound.AddRange(contentPoints);
        //                 _contentsMap.contentPointGrid.UpdateCell(x, y, contentPoints);
        //             }
        //         }
        //     }
        //
        //     return contentPointsFound;
        // }
    }
    
    // This is a shortcut. We use the List<T> but just add a Cloneale interface to it and implement it.
    // todo: will make this code cleaner later. Maybe implement it inside of plugins? Or util?
    public class CloneableList<T> : List<T>, ICloneable 
    {
        public object Clone() {
            CloneableList<T> clonedList = new CloneableList<T>();

            foreach (T item in this) {
                if (item is ICloneable cloneableItem) {
                    clonedList.Add((T)cloneableItem.Clone());
                } else {
                    clonedList.Add(item);
                }
            }

            return clonedList;
        }
    }

}