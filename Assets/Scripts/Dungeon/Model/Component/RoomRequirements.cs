using System;
using Dungeon.Generator;
using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Model {
    public class RoomRequirements {

        private FlexGrid2DString roomMap;
        private ExitMap exitMap;
        private Grid2DString requirements;
        private Grid2DString requirementsPrevious;
        private Vector2Int coordinates;

        public RoomRequirements(Vector2Int thisRoomCoordinates, FlexGrid2DString roomMap, ExitMap exitMap) {
            this.roomMap = roomMap;
            this.exitMap = exitMap;
            this.coordinates = thisRoomCoordinates;
            
            this.requirements = GetRoomRequirementsBasic();
            
            requirementsPrevious = requirements;
        }

        public Grid2DString ToGrid() {
            return requirements;
        }

        private Grid2DString GetRoomRequirementsBasic() {
            // When we take the Mandatory requirements - we care about To and From
            Grid2DString requirementsGrid = new Grid2DString(Consts.Get<int>("RoomSize") + 2);

            getBasicRequirements_Top(this.coordinates, ref requirementsGrid);
            getBasicRequirements_Bottom(this.coordinates, ref requirementsGrid);
            getBasicRequirements_Left(this.coordinates, ref requirementsGrid);
            getBasicRequirements_Right(this.coordinates, ref requirementsGrid);

            return requirementsGrid;
        }

        private void getBasicRequirements_Top(Vector2Int roomCoord, ref Grid2DString requirementsGrid) {
            try {
                if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Top ||
                    exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y+1).mainExitDirection == Exit.SidePosition.Bottom) {
                    int y = Consts.Get<int>("RoomSize") + 1;
                    for (int x = 1; x < Consts.Get<int>("RoomSize") + 1; x++) {
                        requirementsGrid.UpdateCell(x, y, "3");
                    }
                }
            } catch (Exception e) { /*Exception*/ }
        }
        
        private void getBasicRequirements_Bottom(Vector2Int roomCoord, ref Grid2DString requirementsGrid) {
            try {
                if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Bottom ||
                    exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y-1).mainExitDirection == Exit.SidePosition.Top) {
                    int y = 0;
                    for (int x = 1; x < Consts.Get<int>("RoomSize") + 1; x++) {
                        requirementsGrid.UpdateCell(x, y, "3");
                    }
                }
            } catch (Exception e) { /*Exception*/ }
        }
        
        private void getBasicRequirements_Left(Vector2Int roomCoord, ref Grid2DString requirementsGrid) {
            try {
                if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Left ||
                    exitMap._exitMap.GetCellActual(roomCoord.x+1, roomCoord.y).mainExitDirection == Exit.SidePosition.Right) {
                    int x = Consts.Get<int>("RoomSize")+1;
                    for (int y = 1; y < Consts.Get<int>("RoomSize") + 1; y++) {
                        requirementsGrid.UpdateCell(x, y, "3");
                    }
                }
            } catch (Exception e) { /*Exception*/ }
        }
        
        private void getBasicRequirements_Right(Vector2Int roomCoord, ref Grid2DString requirementsGrid) {
            try {
                if (exitMap._exitMap.GetCellActual(roomCoord.x, roomCoord.y).mainExitDirection == Exit.SidePosition.Right ||
                    exitMap._exitMap.GetCellActual(roomCoord.x-1, roomCoord.y).mainExitDirection == Exit.SidePosition.Left) {
                    int x = 0;
                    for (int y = 0; y < Consts.Get<int>("RoomSize") + 1; y++) {
                        requirementsGrid.UpdateCell(x, y, "3");
                    }
                }
            } catch (Exception e) { /*Exception*/ }
        }

        public bool Improve() {
            // ONLY 1 improvement at a time !
            requirementsPrevious = requirements;
            bool foundNewRequirement = false;
            
            foundNewRequirement = getNextRequirement_Top(coordinates);
            if (foundNewRequirement) {
                return true;
            }
            foundNewRequirement = getNextRequirement_Bottom(coordinates);
            if (foundNewRequirement) {
                return true;
            }
            foundNewRequirement = getNextRequirement_Left(coordinates);
            if (foundNewRequirement) {
                return true;
            }
            foundNewRequirement = getNextRequirement_Right(coordinates);
            if (foundNewRequirement) {
                return true;
            }

            return false;
        }

        public void RollBack() {
            requirements = requirementsPrevious;
        }

        private bool getNextRequirement_Top(Vector2Int roomCoord) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y+1);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
                
                int y = Consts.Get<int>("RoomSize") + 1;
                for (int x = 0; x < Consts.Get<int>("RoomSize")+1; x++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        if (requirements.GetCell(x,0) != "") continue;
                        requirements.UpdateCell(x, 0, thisCellContents);
                        return true;
                    }
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }
        
        private bool getNextRequirement_Bottom(Vector2Int roomCoord) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x, roomCoord.y-1);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int y = 0;
                for (int x = 0; x < Consts.Get<int>("RoomSize")+1; x++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents == "") continue;
                    if (requirements.GetCell(x,Consts.Get<int>("RoomSize")+1) != "") continue;
                    requirements.UpdateCell(x, Consts.Get<int>("RoomSize")+1, thisCellContents);
                    return true;
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }
        
        private bool getNextRequirement_Left(Vector2Int roomCoord) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x+1, roomCoord.y);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int x = Consts.Get<int>("RoomSize") + 1;
                for (int y = 0; y < Consts.Get<int>("RoomSize")+1; y++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        if (requirements.GetCell(0, y) != "") continue;
                        requirements.UpdateCell(0, y, thisCellContents);
                        return true;
                    }
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }

        private bool getNextRequirement_Right(Vector2Int roomCoord) {
            try {
                string roomID = roomMap.GetCellActual(roomCoord.x-1, roomCoord.y);
                Room room = RoomGenerator.FindRoomInstanceByID(roomID);
            
                int x = 0;
                for (int y = 1; y < Consts.Get<int>("RoomSize")+1; y++) {
                    string thisCellContents = room.roomLayout.GetCell(x, y);
                    if (thisCellContents != "") {
                        if (requirements.GetCell(Consts.Get<int>("RoomSize")+1, y) != "") continue;
                        requirements.UpdateCell(Consts.Get<int>("RoomSize")+1, y, thisCellContents);
                        return true;
                    }
                }
            } catch (Exception e) { /*ignored*/ }
            
            return false;
        }
    }
}
