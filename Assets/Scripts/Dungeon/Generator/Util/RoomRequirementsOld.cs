using System;
using System.Collections.Generic;
using Dungeon.Generator.Stage;
using Dungeon.Properties.Map.Type;
using Dungeon.Properties.Map.Util;
using Dungeon.Room;
using Grid2DEditor;
using UnityEngine;

namespace Dungeon.Generator.Util {
    public class RoomRequirementsOld {
        
        private Grid2DResizable roomMap;
        private ExitMap exitMap;
        private Grid2D requirements;

        public RoomRequirementsOld(Vector2Int thisRoomCoordinates, Grid2DResizable roomMap, ExitMap exitMap) {
            this.roomMap = roomMap;
            this.exitMap = exitMap;
            requirements = GetRoomRequirements(thisRoomCoordinates);
        }

        public Grid2D ToGrid() {
            return requirements;
        }
        
        private Grid2D GetRoomRequirements(Vector2Int fromCoordinates) {
            // bool isExternalExit = IsRoomAnExternalExit(coordinatesFull);
            Grid2D requirementsGrid = new Grid2D(Consts.RoomSize+2);
            
            // todo: must be 2 NECCESSARY requirements - TO and FROM (3 - soft exit).
            
            // todo: Iterative approach -> implement
            
            requirementsGrid = getRequirements_Top(fromCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Bottom(fromCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Left(fromCoordinates, requirementsGrid);
            requirementsGrid = getRequirements_Right(fromCoordinates, requirementsGrid);
            
            // Now we free the appropriate cells according to exits to allow the space
            requirementsGrid = getRequirements_BasedOnExits(requirementsGrid);

            return requirementsGrid;
        }
        
        private Grid2D getRequirements_Top(Vector2Int roomCoord, Grid2D requirementsGrid) {
            string roomContents = "";
            try { roomContents = roomMap.GetCellActual(roomCoord.x, roomCoord.y+1); } catch (Exception e) {/*ignored*/}
            
            if (int.TryParse(roomContents, out _)) {
                try {
                    string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y+1);
                    RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                    int y = Consts.RoomSize + 1;
                    for (int x = 0; x < Consts.RoomSize+1; x++) {
                        string thisCellContents = room.roomLayout.GetCell(x, y);
                        if (thisCellContents != "") {
                            requirementsGrid.UpdateCell(x, 0, thisCellContents);
                        }
                    }
                    // return UpdateSideSoftExit(requirementsGrid, "horizontal" , 1, Consts.RoomSize+1);
                } catch (Exception e) { /*ignored*/ }
            }
            else {
                try {
                    if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection ==
                        Exit.SidePosition.Top) {
                        int y = Consts.RoomSize + 1;
                        for (int x = 1; x < Consts.RoomSize + 1; x++) {
                            requirementsGrid.UpdateCell(x, y, "3");
                        }
                    }
                } catch (Exception e) { /*ignored*/ }
            }

            return requirementsGrid;
        }

        private Grid2D getRequirements_Bottom(Vector2Int roomCoord, Grid2D requirementsGrid) {
            string roomContents = "";
            try { roomContents = roomMap.GetCellActual(roomCoord.x, roomCoord.y-1); } catch (Exception e) {/*ignored*/}
            
            if (int.TryParse(roomContents, out _)) {
                try {
                    string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y-1);
                    RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                    int y = 0;
                    for (int x = 0; x < Consts.RoomSize+1; x++) {
                        string thisCellContents = room.roomLayout.GetCell(x, y);
                        if (thisCellContents != "") {
                            requirementsGrid.UpdateCell(x, Consts.RoomSize+1, thisCellContents);
                        }
                    }
                    // return UpdateSideSoftExit(requirementsGrid, "horizontal" , 1, Consts.RoomSize+1);
                } catch (Exception e) { /*ignored*/ }
            }
            else {
                try {
                    if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection ==
                        Exit.SidePosition.Bottom) {
                        int y = 0;
                        for (int x = 1; x < Consts.RoomSize + 1; x++) {
                            requirementsGrid.UpdateCell(x, y, "3");
                        }
                    }
                } catch (Exception e) { /*ignored*/ }
            }

            return requirementsGrid;
        }
        
        private Grid2D getRequirements_Left(Vector2Int roomCoord, Grid2D requirementsGrid) {
            string roomContents = "";
            try { roomContents = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y); } catch (Exception e) {/*ignored*/}
            
            if (int.TryParse(roomContents, out _)) {
                try {
                    string roomID = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y);
                    RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                    int x = Consts.RoomSize + 1;
                    for (int y = 0; y < Consts.RoomSize+1; y++) {
                        string thisCellContents = room.roomLayout.GetCell(x, y);
                        if (thisCellContents != "") {
                            requirementsGrid.UpdateCell(0, y, thisCellContents);
                        }
                    }
                    // return UpdateSideSoftExit(requirementsGrid, "horizontal" , 1, Consts.RoomSize+1);
                } catch (Exception e) { /*ignored*/ }
            }
            else {
                try {
                    if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection ==
                        Exit.SidePosition.Left) {
                        int x = 0;
                        for (int y = 1; y < Consts.RoomSize + 1; y++) {
                            requirementsGrid.UpdateCell(x, y, "3");
                        }
                    }
                } catch (Exception e) { /*ignored*/ }
            }
            

            return requirementsGrid;
        }

        private Grid2D getRequirements_Right(Vector2Int roomCoord, Grid2D requirementsGrid) {
            string roomContents = "";
            try { roomContents = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y); } catch (Exception e) {/*ignored*/}
            
            if (int.TryParse(roomContents, out _)) {
                try {
                    string roomID = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y);
                    RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                    int x = 0;
                    for (int y = 1; y < Consts.RoomSize+1; y++) {
                        string thisCellContents = room.roomLayout.GetCell(x, y);
                        if (thisCellContents != "") {
                            requirementsGrid.UpdateCell(Consts.RoomSize+1, y, thisCellContents);
                        }
                    }
                    // return UpdateSideSoftExit(requirementsGrid, "horizontal" , 1, Consts.RoomSize+1);
                } catch (Exception e) { /*ignored*/ }
            }
            else {
                try {
                    if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection ==
                        Exit.SidePosition.Right) {
                        int x = Consts.RoomSize+1;
                        for (int y = 0; y < Consts.RoomSize + 1; y++) {
                            requirementsGrid.UpdateCell(x, y, "3");
                        }
                    }
                } catch (Exception e) { /*ignored*/ }
            }

            return requirementsGrid;
        }


        // private Grid2D UpdateSideSoftExit(Grid2D requirementsGrid, string side, int initX, int initY) {
        //     if (side == "horizontal") {
        //         for (int x = initX; x < initX; x++) {
        //             requirementsGrid.UpdateCell(x, initY, "3");
        //         }
        //     }
        //     
        //     if (side == "vertical") {
        //         for (int y = initY; y < initY; y++) {
        //             requirementsGrid.UpdateCell(initX, y, "3");
        //         }
        //     }
        //
        //     return requirementsGrid;
        // }
        
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