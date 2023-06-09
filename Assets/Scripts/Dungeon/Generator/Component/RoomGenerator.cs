using System;
using System.Collections.Generic;
using Dungeon.Model;
using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Generator {
    public static class RoomGenerator {

        private static ChunkMap _chunkMap;
        private static RoomMap _newRoomMap;
        private static ExitMap _exitMap;

        public static List<Room> roomLayoutsAvailable;

        public static RoomMap GenerateRooms(ChunkMap chunkMap) {
            _chunkMap = chunkMap;
            _newRoomMap = PrebuildRoomMap(new RoomMap (
                    chunkMap.map.getXSize() * Consts.Get<int>("ChunkSize"),
                    chunkMap.map.getYSize() * Consts.Get<int>("ChunkSize")
                ));
            
            _exitMap = new ExitMap((FlexGrid2DString)_newRoomMap.map.Clone(),
                              _newRoomMap.map.getXSize(), 
                              _newRoomMap.map.getYSize());
            
            for (int y = 0; y < _newRoomMap.map.getYSize()-1; y++) {
                for (int x = 0; x < _newRoomMap.map.getXSize()-1; x++) {
                    RoomCoordinatesFull coordinatesFull = GetCoordinatesFull(new Vector2Int(x, y));
                    if (IsRoomEmpty(coordinatesFull)) continue;

                    string selectedRoomID = SelectRoomForThisCell(coordinatesFull.room_RoomMap);
                    // Insert new room ID
                    _newRoomMap.PlaceCellOnMap(coordinatesFull.room_RoomMap, selectedRoomID);
                }
            }

            // _newRoomMap.map.DisplayGrid();
            return _newRoomMap;
        }

        private static RoomMap PrebuildRoomMap(RoomMap roomMap) {
            for (int y = 0; y < roomMap.map.getYSize() - 1; y++) {
                for (int x = 0; x < roomMap.map.getXSize() - 1; x++) {
                    // Vector2Int roomOffsetCoord = new Vector2Int(x-_zeroYOffset, y-_zeroYOffset);
                    RoomCoordinatesFull coordinatesFull = GetCoordinatesFull(new Vector2Int(x, y));
                    
                    // Find corresponding in chunk.
                    // --> Don't forget that we have a Offset in ChunkMap
                    string thisChunkID = _chunkMap.map.GetCellActual(
                        coordinatesFull.chunk_ChunkMap.x, coordinatesFull.chunk_ChunkMap.y);
                        // Because we do GetCellACTUAL -> We will consider negative coordinates as non-negative.
                        //    So 0,0 can be a negative coordinate
                    if (thisChunkID == "") {
                        roomMap.PlaceCellOnMap(coordinatesFull.room_RoomMap, "");
                        continue;
                    }
                    // Correctly takes the Chunk with rooms inside. But does not find the actual roomID here : ...
                    string thisRoomID = ChunkGenerator.FindChunkLayoutByID(thisChunkID).rooms.GetCell(
                        coordinatesFull.room_ThisChunk.x,
                        coordinatesFull.room_ThisChunk.y);
                    
                    roomMap.PlaceCellOnMap(coordinatesFull.room_RoomMap, thisRoomID);
                }
            }

            return roomMap;
        }

        private static RoomCoordinatesFull GetCoordinatesFull(Vector2Int roomCoordinatesOnGrid) {
            
            RoomCoordinatesFull coordinatesFull = new RoomCoordinatesFull {
                room_RoomMap = roomCoordinatesOnGrid,
                
                // Actual -> Does not include offsets which are only made for the sake of abstraction.
                chunk_ChunkMap = TransferRoomToChunkCoordinates(
                    roomCoordinatesOnGrid.x, roomCoordinatesOnGrid.y),
                
                room_ThisChunk = GetInverseInChunkCoordinates(new Vector2Int(
                    roomCoordinatesOnGrid.x < Consts.Get<int>("ChunkSize") ? roomCoordinatesOnGrid.x :
                        roomCoordinatesOnGrid.x - Consts.Get<int>("ChunkSize") * (roomCoordinatesOnGrid.x / Consts.Get<int>("ChunkSize")),
                    roomCoordinatesOnGrid.y < Consts.Get<int>("ChunkSize") ? roomCoordinatesOnGrid.y :
                        roomCoordinatesOnGrid.y - Consts.Get<int>("ChunkSize") * (roomCoordinatesOnGrid.y / Consts.Get<int>("ChunkSize"))))
            };
            
            // Debug.Log("room_ThisChunk : " + coordinatesFull.room_ThisChunk.x + " : " + coordinatesFull.room_ThisChunk.y);
            
            string thisChunkID = _chunkMap.map.GetCellActual(
                coordinatesFull.chunk_ChunkMap.x, coordinatesFull.chunk_ChunkMap.y);
            try { coordinatesFull.chunk = ChunkGenerator.FindChunkLayoutByID(thisChunkID); } catch (ArgumentException e) { }

            return coordinatesFull;
        }

        private static Vector2Int GetInverseInChunkCoordinates(Vector2Int actualInChunkCoordinates) {
            int invertedX = Consts.Get<int>("ChunkSize") - 1 - actualInChunkCoordinates.x;
            int invertedY = Consts.Get<int>("ChunkSize") - 1 - actualInChunkCoordinates.y;

            return new Vector2Int(invertedX, invertedY);
        }
        
        private static bool IsRoomEmpty(RoomCoordinatesFull coordinatesFull) {
            if (coordinatesFull.chunk == null) {
                return true;
            }
            string thisRoomType = coordinatesFull.chunk.rooms.GetCell(coordinatesFull.room_ThisChunk.x, coordinatesFull.room_ThisChunk.y);
            return thisRoomType == "";
        }

        private static Vector2Int TransferRoomToChunkCoordinates(int roomX, int roomY) {
            int chunkSize = Consts.Get<int>("ChunkSize");
            int chunkX = 0;
            int chunkY = 0;

            while (roomX >= 4) {
                roomX -= 4;
                chunkX++;
            }

            while (roomY >= 4) {
                roomY -= 4;
                chunkY++;
            }

            return new Vector2Int(chunkX, chunkY);
        }

        private static string SelectRoomForThisCell(Vector2Int cellCoordinates) {
            // Here we initialise requirements. And need to ONLY find the Soft Exits (neccessary)
            RoomRequirements requirements = new RoomRequirements(cellCoordinates, _newRoomMap.map, _exitMap);
            List<string> roomsAvailable = GetRoomBasedOnRequirements(requirements.ToGrid());
            if (roomsAvailable.Count == 0) {
                // todo : can't find appropriate rooms ? roomIDs.Count == 0
                return "-1";
            }

            bool improveStatus = true;
            while (roomsAvailable.Count != 0 && improveStatus == true) {
                improveStatus = requirements.Improve();
                roomsAvailable = GetRoomBasedOnRequirements(requirements.ToGrid());
            }

            if (improveStatus == false) {
                // Then we don't need to roll back the requirements
            }

            requirements.RollBack();
            roomsAvailable = GetRoomBasedOnRequirements(requirements.ToGrid());
            return roomsAvailable[DungeonGenerator.UseSeed(roomsAvailable.Count-1)];

        }

        private static List<string> GetRoomBasedOnRequirements(Grid2DString requirements) {
            List<string> roomLayouts = new List<string>();

            foreach (Room assessedRoomLayout in roomLayoutsAvailable) {
                // Must check if it includes all the exits
                bool isSimilar = true;
                for (int y = 0; y < requirements.Size-1; y++) {
                    for (int x = 0; x < requirements.Size-1; x++) {
                        if (requirements.GetCell(x, y) == "2") { // Check cells that MUST have exits
                            if (assessedRoomLayout.roomLayout.GetCell(x,y) != "2") {
                                isSimilar = false;
                            }
                        }
                        if (requirements.GetCell(x, y) == "0") { // Check cells that MUST be empty
                            if (assessedRoomLayout.roomLayout.GetCell(x,y) != "0") {
                                isSimilar = false;
                            }
                        }
                    }
                }

                isSimilar = CheckNotStrictExits(requirements, assessedRoomLayout);

                if (isSimilar) {
                    roomLayouts.Add(assessedRoomLayout.roomID);
                }
            }
            
            return roomLayouts;
        }

        private static bool CheckNotStrictExits(Grid2DString requirements, Room assessedRoomLayout) {
            bool allExitsCoorect = true;
            // todo: remember that requirements is Consts.Get<int>("RoomSize") + 2
            
            // Top - requirements Grid
            if (requirements.GetCell(1, Consts.Get<int>("RoomSize")+1) == "3") {
                // Then we need exit on ANY cell on this side
                allExitsCoorect = false;
                for (int x = 1; x < Consts.Get<int>("RoomSize")+1; x++) {
                    if (assessedRoomLayout.roomLayout.GetCell(x, 0) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(x, 0) == "3" ) {
                        allExitsCoorect = true;
                    }
                }
            }

            if (!allExitsCoorect) return false;
            
            // Bottom
            if (requirements.GetCell(1, 0) == "3") {
                // Then we need exit on ANY cell on this side
                allExitsCoorect = false;
                for (int x = 1; x < Consts.Get<int>("RoomSize"); x++) {
                    if (assessedRoomLayout.roomLayout.GetCell(x, Consts.Get<int>("RoomSize")+1) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(x, Consts.Get<int>("RoomSize")+1) == "3" ) {
                        allExitsCoorect = true;
                    }
                }
            }
            
            if (!allExitsCoorect) return false;
            
            // Left
            if (requirements.GetCell(0, 1) == "3") {
                // Then we need exit on ANY cell on this side
                allExitsCoorect = false;
                for (int y = 1; y < Consts.Get<int>("RoomSize"); y++) {
                    if (assessedRoomLayout.roomLayout.GetCell(Consts.Get<int>("RoomSize") + 1, y) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(Consts.Get<int>("RoomSize") + 1, y) == "3" ) {
                        allExitsCoorect = true;
                    }
                }
            }
            
            if (!allExitsCoorect) return false;
            
            // Right
            if (requirements.GetCell(Consts.Get<int>("RoomSize")+1, 1) == "3") {
                // Then we need exit on ANY cell on this side
                allExitsCoorect = false;
                for (int y = 1; y < Consts.Get<int>("RoomSize"); y++) {
                    if (assessedRoomLayout.roomLayout.GetCell(0, y) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(0, y) == "3" ) {
                        allExitsCoorect = true;
                    }
                }
            }

            return allExitsCoorect;
        }

        // private static bool IsExternalExit(int x, int y) {
        //     // First detect chunk
        //     Vector2Int thisChunkCoordindates = GetChunkCoordinatesBasedOnRoom(x, y);
        //     string thisChunkID = _chunkMap.map.GetCell(thisChunkCoordindates.x, thisChunkCoordindates.y);
        //
        //     try { ChunkLayout thisChunkLayout = ChunkGenerator.FindChunkLayoutByID(thisChunkID); }
        //     catch (ArgumentException e) {
        //         throw new ArgumentException("Empty room contents");
        //     }
        //     // Then find an index in chunk's 2DResizableGrid.
        //     // if E then return True
        //     return true;
        // }

        private static  Vector2Int GetChunkCoordinatesBasedOnRoom(int x, int y) {
            int chunkSize = Consts.Get<int>("ChunkSize");
            Vector2Int chunkCoord = new Vector2Int();

            if (x < chunkSize) {
                chunkCoord.x = 0;
            }
            if (y < chunkSize) {
                chunkCoord.y = 0;
            }
            
            chunkCoord.x = (x - (x % chunkSize) / chunkSize) - 1;
            chunkCoord.y = (y - (y % chunkSize) / chunkSize) - 1;

            return chunkCoord;
        }

        public static Room FindRoomInstanceByID(string id) {
            bool isParsableID = int.TryParse(id, out _); // _ means that we don't intend to use the out result
            // Check if this is a string
            // Otherwise, it could be an E or an R
            
            if (id == "" || !isParsableID) {
                throw new ArgumentException("Chunk ID can't be empty");
            }
            return roomLayoutsAvailable.FindAll( room => room.roomID == id)[0];
        }
    }
}

public struct RoomCoordinatesFull {
    public Vector2Int room_RoomMap;
    public Vector2Int chunk_ChunkMap;
    public Vector2Int room_ThisChunk;
    public Chunk chunk;
}