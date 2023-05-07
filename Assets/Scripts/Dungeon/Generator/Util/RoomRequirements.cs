using System;
using System.Collections.Generic;
using Dungeon.Properties.Map.Type;
using Dungeon.Room;
using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Generator.Util {
    public class RoomRequirements {
        
        private RoomCoordinatesFull coordinatesFull;
        private Grid2D requirements;
        private RoomMap roomMap;
        private List<RoomInstance> roomLayoutsAvailable;

        public RoomRequirements(RoomCoordinatesFull coordinatesFullParams, RoomMap roomMapParams, List<RoomInstance> roomLayoutsAvailable) {
            coordinatesFull = coordinatesFullParams;
            roomMap = roomMapParams;
            requirements = GetRoomRequirements(coordinatesFull);
        }

        public Grid2D ToGrid() {
            return requirements;
        }
        
        private Grid2D GetRoomRequirements(RoomCoordinatesFull coordinatesFull) {
            bool isExternalExit = IsRoomAnExternalExit(coordinatesFull);
            Grid2D requirementsGrid = new Grid2D(Consts.RoomSize+2);
            Vector2Int thisRoomCoordinates = new Vector2Int(
                coordinatesFull.room_RoomMap.x, coordinatesFull.room_RoomMap.y);
            
            requirementsGrid = getRequirements_Top(thisRoomCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Bottom(thisRoomCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Left(thisRoomCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Right(thisRoomCoordinates, requirementsGrid);
            
            // Now we free the appropriate cells according to exits to allow the space
            requirementsGrid = getRequirements_BasedOnExits(requirementsGrid);

            return requirementsGrid;
        }
        
        private Grid2D getRequirements_Top(Vector2Int roomCoord, Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.map.GetCellActual(roomCoord.x, roomCoord.y+1);
                RoomInstance room = FindRoomInstanceByID(roomID);
                
                int y = Consts.RoomSize + 1;
                for (int x = 1; x < Consts.RoomSize+1; x++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    requirementsGrid.UpdateCell(x, y, thisCellContents);
                }
                
                return UpdateSideSoftExit(requirementsGrid, "horizontal" , 1, Consts.RoomSize+1);
            } catch (Exception e) {
                // Will throw exception if :
                //  - no room above
                //  - out of range 
                return requirementsGrid;
            } 
        }
        
        private Grid2D getRequirements_Bottom(Vector2Int roomCoord, Grid2D requirementsGrid) {
            try {
                string roomID =roomMap.map.GetCellActual(roomCoord.x, roomCoord.y-1);
                RoomInstance room = FindRoomInstanceByID(roomID);
                
                int y = 0;
                for (int x = 1; x < Consts.RoomSize+1; x++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    requirementsGrid.UpdateCell(x, y, thisCellContents);
                }
                
                return UpdateSideSoftExit(requirementsGrid, "horizontal", 1, 0);
            } catch (Exception e) {
                // Will throw exception if :
                //  - no room above
                //  - out of range 
                return requirementsGrid;
            } 
        }
        
        private Grid2D getRequirements_Left(Vector2Int roomCoord, Grid2D requirementsGrid) {
            try {
                string roomID =roomMap.map.GetCellActual(roomCoord.x-1, roomCoord.y);
                RoomInstance room = FindRoomInstanceByID(roomID);
                
                int x = 0;
                for (int y = 1; y < Consts.RoomSize+1; y++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    requirementsGrid.UpdateCell(x, y, thisCellContents);
                }
                
                return UpdateSideSoftExit(requirementsGrid, "vertical", 0, 1);
            } catch (Exception e) {
                // Will throw exception if :
                //  - no room above
                //  - out of range 
                return requirementsGrid;
            } 
        }

        private Grid2D getRequirements_Right(Vector2Int roomCoord, Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.map.GetCellActual(roomCoord.x+1, roomCoord.y);
                RoomInstance room = FindRoomInstanceByID(roomID);
                
                int x = Consts.RoomSize+1;
                for (int y = 1; y < Consts.RoomSize+1; y++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    requirementsGrid.UpdateCell(x, y, thisCellContents);
                }
                
                return UpdateSideSoftExit(requirementsGrid, "vertical", Consts.RoomSize+1, 1);
            } catch (Exception e) {
                // Will throw exception if :
                //  - no room above
                //  - out of range 
                return requirementsGrid;
            } 
        }


        private Grid2D UpdateSideSoftExit(Grid2D requirementsGrid, string side, int initX, int initY) {
            if (side == "horizontal") {
                for (int x = initX; x < initX; x++) {
                    requirementsGrid.UpdateCell(x, initY, "3");
                }
            }
            
            if (side == "vertical") {
                for (int y = initY; y < initY; y++) {
                    requirementsGrid.UpdateCell(initX, y, "3");
                }
            }

            return requirementsGrid;
        }
        
        private Grid2D getRequirements_BasedOnExits(Grid2D requirementsGrid) {
            // todo: Finish later
            return requirementsGrid;
        }
        
        private bool IsRoomAnExternalExit(RoomCoordinatesFull coordinatesFull) {
            string thisRoomType = coordinatesFull.chunkLayout.rooms.GetCell(coordinatesFull.room_ThisChunk.x, coordinatesFull.room_ThisChunk.y);
            return thisRoomType == "E";
        }
        
        private  RoomInstance FindRoomInstanceByID(string id) {
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