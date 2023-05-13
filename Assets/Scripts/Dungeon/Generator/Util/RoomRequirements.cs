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
        private ExitMap exitMap;
        private Grid2D requirements;
        private Grid2D requirementsPrevious;
        private Vector2Int coordinates;

        public RoomRequirements(Vector2Int thisRoomCoordinates, Grid2DResizable roomMap, ExitMap exitMap) {
            this.roomMap = roomMap;
            this.exitMap = exitMap;
            this.coordinates = thisRoomCoordinates;
            
            this.requirements = GetRoomRequirementsMandatory();
            
            requirementsPrevious = requirements;
        }

        public Grid2D ToGrid() {
            return requirements;
        }

        private Grid2D GetRoomRequirementsMandatory() {
            // When we take the Mandatory requirements - we care about To and From
            Grid2D requirementsGrid = new Grid2D(Consts.RoomSize + 2);

            requirementsGrid = getMandatoryRequirements_Top(this.coordinates, requirementsGrid);
            requirementsGrid = getMandatoryRequirements_Bottom(this.coordinates, requirementsGrid);
            requirementsGrid = getMandatoryRequirements_Left(this.coordinates, requirementsGrid);
            requirementsGrid = getMandatoryRequirements_Right(this.coordinates, requirementsGrid);

            return requirementsGrid;
        }

        private Grid2D getMandatoryRequirements_Top(Vector2Int roomCoord, Grid2D requirementsGrid) {
            if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Top ||
                exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y+1).mainExitDirection == Exit.SidePosition.Bottom) {
                int y = Consts.RoomSize + 1;
                for (int x = 1; x < Consts.RoomSize + 1; x++) {
                    requirementsGrid.UpdateCell(x, y, "3");
                }
            }

            return requirementsGrid;
        }
        
        private Grid2D getMandatoryRequirements_Bottom(Vector2Int roomCoord, Grid2D requirementsGrid) {
            if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Bottom ||
                exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y-1).mainExitDirection == Exit.SidePosition.Top) {
                int y = 0;
                for (int x = 1; x < Consts.RoomSize + 1; x++) {
                    requirementsGrid.UpdateCell(x, y, "3");
                }
            }

            return requirementsGrid;
        }
        
        private Grid2D getMandatoryRequirements_Left(Vector2Int roomCoord, Grid2D requirementsGrid) {
            if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Left ||
                exitMap._exitMap.GetCellActual(roomCoord.x+1, roomCoord.y).mainExitDirection == Exit.SidePosition.Right) {
                int x = 0;
                for (int y = 1; y < Consts.RoomSize + 1; y++) {
                    requirementsGrid.UpdateCell(x, y, "3");
                }
            }

            return requirementsGrid;
        }
        
        private Grid2D getMandatoryRequirements_Right(Vector2Int roomCoord, Grid2D requirementsGrid) {
            if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Right ||
                exitMap._exitMap.GetCellActual(roomCoord.x-1, roomCoord.y).mainExitDirection == Exit.SidePosition.Left) {
                int x = Consts.RoomSize+1;
                for (int y = 0; y < Consts.RoomSize + 1; y++) {
                    requirementsGrid.UpdateCell(x, y, "3");
                }
            }

            return requirementsGrid;
        }

        public bool Improve() {
            // ONLY 1 improvement at a time !
            Grid2D requirementsGrid = new Grid2D(Consts.RoomSize + 2);
            bool foundNewRequirement = false;
            
            foundNewRequirement = getNextRequirement_Top(coordinates, ref requirementsGrid);
            foundNewRequirement = getNextRequirement_Bottom(coordinates, ref requirementsGrid);
            foundNewRequirement = getNextRequirement_Left(coordinates, ref requirementsGrid);
            foundNewRequirement = getNextRequirement_Right(coordinates, ref requirementsGrid);

            if (!foundNewRequirement) return false;
            
            requirementsPrevious = requirements;
            requirements = requirementsGrid;
            return true;
        }

        public void RollBack() {
            requirements = requirementsPrevious;
        }

        private bool getNextRequirement_Top(Vector2Int roomCoord, ref Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y+1);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int y = Consts.RoomSize + 1;
                for (int x = 0; x < Consts.RoomSize+1; x++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        requirementsGrid.UpdateCell(x, 0, thisCellContents);
                        return true;
                    }
                }
            } catch (IndexOutOfRangeException e) { /*ignored*/ }
            
            return false;
        }
        
        private bool getNextRequirement_Bottom(Vector2Int roomCoord, ref Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y-1);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int y = 0;
                for (int x = 0; x < Consts.RoomSize+1; x++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        requirementsGrid.UpdateCell(x, Consts.RoomSize+1, thisCellContents);
                        return true;
                    }
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }
        
        private bool getNextRequirement_Left(Vector2Int roomCoord, ref Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int x = Consts.RoomSize + 1;
                for (int y = 0; y < Consts.RoomSize+1; y++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        requirementsGrid.UpdateCell(0, y, thisCellContents);
                        return true;
                    }
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }

        private bool getNextRequirement_Right(Vector2Int roomCoord, ref Grid2D requirementsGrid) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y);
                RoomInstance room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int x = 0;
                for (int y = 1; y < Consts.RoomSize+1; y++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        requirementsGrid.UpdateCell(Consts.RoomSize+1, y, thisCellContents);
                        return true;
                    }
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }
    }
}
