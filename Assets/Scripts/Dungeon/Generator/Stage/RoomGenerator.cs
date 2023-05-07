using System;
using System.Collections.Generic;
using Dungeon.Chunk;
using Dungeon.Properties.Map.Type;
using Dungeon.Room;
using Dungeon.Generator.Util;
using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Generator.Stage {
    public static class RoomGenerator {

        private static ChunkMap _chunkMap;
        private static int _zeroYOffset;
        private static RoomMap _newRoomMap;
        public static List<RoomInstance> roomLayoutsAvailable;
        public static string _seed;

        public static  RoomMap GenerateRooms(string seed, ChunkMap chunkMap) {
            _seed = seed;
            _chunkMap = chunkMap;
            _zeroYOffset = chunkMap.map.zeroYOffset;
            _newRoomMap = new RoomMap(
                    chunkMap.map.getXSize() * Consts.ChunkSize,
                    chunkMap.map.getYSize() * Consts.ChunkSize
                ); PrebuildRoomMap();
            
            // I think we will need to fill _newRoomMap with roomExistense first.
            //  This means that before finding roomRequirements we must know each place on the map
            //   Where the RoomInstance should be placed. 
            // Or maybe we just check that cell every time from _chinkMap because we have CoordinatesFull

            for (int y = 0; y < _newRoomMap.map.getYSize()-1; y++) {
                for (int x = 0; x < _newRoomMap.map.getXSize()-1; x++) {
                    // Debug.Log("Finding room for:  " + x + " : " + y);
                    
                    RoomCoordinatesFull coordinatesFull = GetCoordinatesFull(new Vector2Int(x, y));
                    if (IsRoomEmpty(coordinatesFull)) {
                        continue; // Will then go through each empty room to fill it with something else.
                    }
                    
                    // Grid2D requirements = GetRoomRequirements(coordinatesFull); 
                    RoomRequirements requirements = new RoomRequirements(
                        coordinatesFull, _newRoomMap, roomLayoutsAvailable);
                    List<string> roomIDs = GetRoomBasedOnRequirements(requirements.ToGrid());
                    if (roomIDs.Count == 0) {
                        // todo : can't find appropriate rooms ? roomIDs.Count == 0
                        continue;
                    }
                    string chosenRoomID = roomIDs[UseSeed(roomIDs.Count)];
                    // Debug.Log("Chose new room ID: " + chosenRoomID);
                    
                    // Insert new room ID
                    _newRoomMap.PlaceCellOnMap(coordinatesFull.room_RoomMap, chosenRoomID);
                }
            }

            _newRoomMap.map.DisplayGrid();
            return _newRoomMap;
        }

        private static void PrebuildRoomMap() {
            for (int y = 0; y < _newRoomMap.map.getYSize() - 1; y++) {
                for (int x = 0; x < _newRoomMap.map.getXSize() - 1; x++) {
                    // Vector2Int roomOffsetCoord = new Vector2Int(x-_zeroYOffset, y-_zeroYOffset);
                    RoomCoordinatesFull coordinatesFull = GetCoordinatesFull(new Vector2Int(x, y));
                    
                    // Find corresponding in chunk.
                    // --> Don't forget that we have a Offset in ChunkMap
                    string thisChunkID = _chunkMap.map.GetCellActual(
                        coordinatesFull.chunk_ChunkMap.x, coordinatesFull.chunk_ChunkMap.y);
                        // Because we do GetCellACTUAL -> We will consider negative coordinates as non-negative.
                        //    So 0,0 can be a negative coordinate
                    if (thisChunkID == "") {
                        _newRoomMap.PlaceCellOnMap(coordinatesFull.room_RoomMap, "");
                        continue;
                    }
                    // Correctly takes the Chunk with rooms inside. But does not find the actual roomID here : ...
                    string thisRoomID = ChunkGenerator.FindChunkLayoutByID(thisChunkID).rooms.GetCell(
                        coordinatesFull.room_ThisChunk.x,
                        coordinatesFull.room_ThisChunk.y);
                    
                    _newRoomMap.PlaceCellOnMap(coordinatesFull.room_RoomMap, thisRoomID);
                }
            }
        }

        private static RoomCoordinatesFull GetCoordinatesFull(Vector2Int roomCoordinatesOnGrid) {
            
            RoomCoordinatesFull coordinatesFull = new RoomCoordinatesFull {
                room_RoomMap = roomCoordinatesOnGrid,
                
                // Actual -> Does not include offsets which are only made for the sake of abstraction.
                chunk_ChunkMap = TransferRoomToChunkCoordinates(
                    roomCoordinatesOnGrid.x, roomCoordinatesOnGrid.y),
                
                room_ThisChunk = GetInverseInChunkCoordinates(new Vector2Int(
                    roomCoordinatesOnGrid.x < Consts.ChunkSize ? roomCoordinatesOnGrid.x :
                        roomCoordinatesOnGrid.x - Consts.ChunkSize * (roomCoordinatesOnGrid.x / Consts.ChunkSize),
                    roomCoordinatesOnGrid.y < Consts.ChunkSize ? roomCoordinatesOnGrid.y :
                        roomCoordinatesOnGrid.y - Consts.ChunkSize * (roomCoordinatesOnGrid.y / Consts.ChunkSize)))
            };
            
            // Debug.Log("room_ThisChunk : " + coordinatesFull.room_ThisChunk.x + " : " + coordinatesFull.room_ThisChunk.y);
            
            string thisChunkID = _chunkMap.map.GetCellActual(
                coordinatesFull.chunk_ChunkMap.x, coordinatesFull.chunk_ChunkMap.y);
            try { coordinatesFull.chunkLayout = ChunkGenerator.FindChunkLayoutByID(thisChunkID); } catch (ArgumentException e) { }

            return coordinatesFull;
        }

        private static Vector2Int GetInverseInChunkCoordinates(Vector2Int actualInChunkCoordinates) {
            int invertedX = Consts.ChunkSize - 1 - actualInChunkCoordinates.x;
            int invertedY = Consts.ChunkSize - 1 - actualInChunkCoordinates.y;

            return new Vector2Int(invertedX, invertedY);
        }
        
        private static bool IsRoomEmpty(RoomCoordinatesFull coordinatesFull) {
            if (coordinatesFull.chunkLayout == null) {
                return true;
            }
            string thisRoomType = coordinatesFull.chunkLayout.rooms.GetCell(coordinatesFull.room_ThisChunk.x, coordinatesFull.room_ThisChunk.y);
            return thisRoomType == "";
        }

        private static Vector2Int TransferRoomToChunkCoordinates(int roomX, int roomY) {
            int chunkSize = Consts.ChunkSize;
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

        private static List<string> GetRoomBasedOnRequirements(Grid2D requirements) {
            List<string> roomLayouts = new List<string>();

            foreach (RoomInstance assessedRoomLayout in roomLayoutsAvailable) {
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

        private static bool CheckNotStrictExits(Grid2D requirements, RoomInstance assessedRoomLayout) {
            bool anyExit = true;
            // todo: remember that requirements is Consts.roomSize + 1 !
            
            // Top
            if (requirements.GetCell(0, Consts.RoomSize + 1) == "3") {
                // Then we need exit on ANY cell on this side
                anyExit = false;
                for (int x = 1; x < Consts.RoomSize; x++) {
                    if (assessedRoomLayout.roomLayout.GetCell(x, Consts.RoomSize + 1) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(x, Consts.RoomSize + 1) == "3" ) {
                        anyExit = true;
                    }
                }
            }
            
            // Bottom
            if (requirements.GetCell(0, 0) == "3") {
                // Then we need exit on ANY cell on this side
                anyExit = false;
                for (int x = 1; x < Consts.RoomSize; x++) {
                    if (assessedRoomLayout.roomLayout.GetCell(x, 0) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(x, 0) == "3" ) {
                        anyExit = true;
                    }
                }
            }
            
            // Left
            if (requirements.GetCell(0, 0) == "3") {
                // Then we need exit on ANY cell on this side
                anyExit = false;
                for (int y = 1; y < Consts.RoomSize; y++) {
                    if (assessedRoomLayout.roomLayout.GetCell(0, y) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(0, y) == "3" ) {
                        anyExit = true;
                    }
                }
            }
            
            // Right
            if (requirements.GetCell(0, Consts.RoomSize + 1) == "3") {
                // Then we need exit on ANY cell on this side
                anyExit = false;
                for (int y = 1; y < Consts.RoomSize; y++) {
                    if (assessedRoomLayout.roomLayout.GetCell(Consts.RoomSize + 1, y) == "2" || 
                        assessedRoomLayout.roomLayout.GetCell(Consts.RoomSize + 1, y) == "3" ) {
                        anyExit = true;
                    }
                }
            }

            return anyExit;
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
            int chunkSize = Consts.ChunkSize;
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

        private static int UseSeed(int choiceCount) {
            // int seedNumber = (int)_seed[0];
            char firstChar = _seed[0];
            int seedNumber = int.Parse(firstChar.ToString());

            _seed = _seed.Substring(1, _seed.Length - 1) + seedNumber;

            while (choiceCount <= seedNumber) {
                seedNumber = seedNumber - choiceCount;
                if (seedNumber < 0) {
                    seedNumber = 0;
                }
            }
            
            return seedNumber;
        }
    }
}

public struct RoomCoordinatesFull {
    public Vector2Int room_RoomMap;
    public Vector2Int chunk_ChunkMap;
    public Vector2Int room_ThisChunk;
    public ChunkLayout chunkLayout;
}