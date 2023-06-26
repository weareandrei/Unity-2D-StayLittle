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
            
            // Insert a function to DisableWrongExits() here

            _contentsMap = new ContentsMap();
            
            _contentsMap.contentPointGrid = new FlexGrid2DSpecial<CloneableList<ContentPointData>>(
                _roomMap.map.getXSize(),
                _roomMap.map.getYSize(),
                new CloneableList<ContentPointData>()
            );

            _contentsMap.contentPointsAll = FindAllContentPoints(); // todo: maybe CollectibleContentPoints only ?
            _contentsMap.contentPointsUsed = ContentPointSelector();

            return _contentsMap;
        }

        private static List<ContentPointData> FindAllContentPoints() {
            List<ContentPointData> contentPointsFound = new List<ContentPointData>();
            
            for (int y = 0; y < _roomMap.map.getYSize() - 1; y++) {
                for (int x = 0; x < _roomMap.map.getXSize() - 1; x++) {
                    string thisRoomID = _roomMap.map.GetCellActual(x, y);
                    if (thisRoomID != "") {
                        CloneableList<ContentPointData> contentPoints = FindThisRoomContentPoints(thisRoomID);
                        contentPointsFound.AddRange(contentPoints);
                        _contentsMap.contentPointGrid.UpdateCell(x, y, contentPoints);
                    }
                }
            }

            return contentPointsFound;
        }

        private static CloneableList<ContentPointData> FindThisRoomContentPoints(string roomID) {
            Room thisRoom = RoomGenerator.FindRoomInstanceByID(roomID);
            return Room.GetRoomContentPoints(thisRoom);
        }

        private static List<ContentPointData> ContentPointSelector() {
            return _contentsMap.contentPointsAll;
        }
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