using System;
using System.Collections.Generic;
using Dungeon.Generator.Stage;
using Dungeon.Properties.Map.Type;
using Dungeon.Properties.Map.Util;
using Dungeon.Room;
using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Generator.Util {
    public class RoomRequirements {
        
        private Grid2DResizable roomMap;
        private Grid2D requirements;

        public RoomRequirements(Vector2Int thisRoomCoordinates, Grid2DResizable roomMap) {
            this.roomMap = roomMap;
            requirements = GetRoomRequirements(thisRoomCoordinates);
        }

        public Grid2D ToGrid() {
            return requirements;
        }
        
        private Grid2D GetRoomRequirements(Vector2Int fromCoordinates) {
            // bool isExternalExit = IsRoomAnExternalExit(coordinatesFull);
            Grid2D requirementsGrid = new Grid2D(Consts.RoomSize+2);
            
            requirementsGrid = getRequirements_Top(fromCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Bottom(fromCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Left(fromCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Right(fromCoordinates, requirementsGrid);
            
            // Now we free the appropriate cells according to exits to allow the space
            requirementsGrid = getRequirements_BasedOnExits(requirementsGrid);

            return requirementsGrid;
        }
        
        private Grid2D getRequirements_Top(Vector2Int roomCoord, Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y+1);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int y = Consts.RoomSize + 1;
                for (int x = 0; x < Consts.RoomSize+1; x++) {
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
                string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y-1);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int y = 0;
                for (int x = 0; x < Consts.RoomSize+1; x++) {
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
                string roomID = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int x = 0;
                for (int y = 0; y < Consts.RoomSize+1; y++) {
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
                string roomID = roomMap.GetCellActual(roomCoord.x+1, roomCoord.y);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int x = Consts.RoomSize+1;
                for (int y = 0; y < Consts.RoomSize+1; y++) {
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

        // private bool IsRoomAnExternalExit(RoomCoordinatesFull coordinatesFull) {
        //     string thisRoomType = coordinatesFull.chunkLayout.rooms.GetCell(coordinatesFull.room_ThisChunk.x, coordinatesFull.room_ThisChunk.y);
        //     return thisRoomType == "E";
        // }
    }
}