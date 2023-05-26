using System;
using System.Collections.Generic;
using Dungeon.Generator;
using Dungeon.Generator.Util;
using UnityEngine;
using Dungeon.Properties.Map.Util;
using Dungeon.Room;

namespace Dungeon.Properties.Map.Type {
    public class ExitMap {
        
        // We will remove cells from roomMap that were already used
        private Grid2DResizable _roomMap;
        public Grid2DExit _exitMap;
        private List<Exit.PossibleExit> exitsAvailable = new List<Exit.PossibleExit>();

        public ExitMap(Grid2DResizable roomMap, int width, int height) {
            _roomMap = roomMap;
            _exitMap = new Grid2DExit(width, height);
            
            Vector2Int entranceCoordinatesActual = GetEntranceCoordinates();
            BuildExitMap(entranceCoordinatesActual, entranceCoordinatesActual);
        }

        private void BuildExitMap(Vector2Int atCoordinates, Vector2Int fromCoordinates) {
            Exit.ExitMapCell mapCell = new Exit.ExitMapCell(new List<Exit.SidePosition>(), Exit.SidePosition.None);
            
            BinaryRequirements binaryRequirements = GetRoomBinaryRequirements(atCoordinates);

            if (binaryRequirements.top) {
                Exit.SidePosition exitPositionFound = Exit.SidePosition.Top;
                mapCell.exits.Add(Exit.SidePosition.Top);
                exitsAvailable.Add(new Exit.PossibleExit(atCoordinates.x, atCoordinates.y, exitPositionFound));
            } if (binaryRequirements.bottom) {
                Exit.SidePosition exitPositionFound = Exit.SidePosition.Bottom;
                mapCell.exits.Add(exitPositionFound);
                exitsAvailable.Add(new Exit.PossibleExit(atCoordinates.x, atCoordinates.y, exitPositionFound));
            } if (binaryRequirements.right) {
                Exit.SidePosition exitPositionFound = Exit.SidePosition.Right;
                mapCell.exits.Add(exitPositionFound);
                exitsAvailable.Add(new Exit.PossibleExit(atCoordinates.x, atCoordinates.y, exitPositionFound));
            } if (binaryRequirements.left) {
                Exit.SidePosition exitPositionFound = Exit.SidePosition.Left;
                mapCell.exits.Add(exitPositionFound);
                exitsAvailable.Add(new Exit.PossibleExit(atCoordinates.x, atCoordinates.y, exitPositionFound));
            }
            
            mapCell.mainExitDirection = CalculateDirectionBasedOnCoordinates(atCoordinates, fromCoordinates);

            _exitMap.UpdateCell(atCoordinates, mapCell);

            if (exitsAvailable is {Count: > 0}) {
                // Maybe pick a random exit from exitsAvailable using seed?
                // BuildExitMap();
                Exit.PossibleExit possibleExit = exitsAvailable[Generator.Generator.UseSeed(exitsAvailable.Count-1)];
                exitsAvailable.Remove(possibleExit);
                _roomMap.UpdateCell(atCoordinates.x, atCoordinates.y, "");
                BuildExitMap(GetAtCoordinatesParam(possibleExit), new Vector2Int(possibleExit.x, possibleExit.y));
            }
        }

        private BinaryRequirements GetRoomBinaryRequirements(Vector2Int coordinates) {
            BinaryRequirements requirements = new BinaryRequirements();

            try {
                if (_roomMap.GetCellActual(coordinates.x, coordinates.y + 1) != "") {
                    requirements.top = true;
                } 
            } catch (Exception e) { Debug.Log(e); }
            
            try {
                if (_roomMap.GetCellActual(coordinates.x, coordinates.y - 1) != "") {
                    requirements.bottom = true;
                }
            } catch (Exception e) { Debug.Log(e); }
            
            try {
                if (_roomMap.GetCellActual(coordinates.x - 1, coordinates.y) != "") {
                    requirements.right = true;
                }
            } catch (Exception e) { Debug.Log(e); }
            
            try {
                if (_roomMap.GetCellActual(coordinates.x + 1, coordinates.y) != "") {
                    requirements.left = true;
                }
            } catch (Exception e) { Debug.Log(e); }
            
            return requirements;
        }

        private Vector2Int GetAtCoordinatesParam(Exit.PossibleExit possibleExit) {
            switch (possibleExit.position) {
                case Exit.SidePosition.Top :
                    return new Vector2Int(possibleExit.x, possibleExit.y + 1); 
                case Exit.SidePosition.Bottom :
                    return new Vector2Int(possibleExit.x, possibleExit.y - 1); 
                case Exit.SidePosition.Right :
                    return new Vector2Int(possibleExit.x-1, possibleExit.y); 
                case Exit.SidePosition.Left :
                    return new Vector2Int(possibleExit.x+1, possibleExit.y); 
            }

            throw new Exception("Unknown exception in GetAtCoordinatesParam");
        }

        // Entrance Room is always at the chunk that is at 0,0 (depending on offset)
        private Vector2Int GetEntranceCoordinates() {
            int x = 0;
            for (int y = 0; y < Consts.ChunkSize-1; y++) {
                if (_roomMap.GetCell(x,(_roomMap.zeroYOffset * Consts.ChunkSize) + y) == "E") {
                    return new Vector2Int(x, (_roomMap.zeroYOffset * Consts.ChunkSize)+y);
                }
            }

            return new Vector2Int(0,0);
        }
            // But can be at the different place at that chunk. But always at the left
            
            

        private Exit.SidePosition CalculateDirectionBasedOnCoordinates(Vector2Int atCoordinates, Vector2Int fromCoordinates) {
            if (atCoordinates.x > fromCoordinates.x) {
                return Exit.SidePosition.Right;
            }
            if (atCoordinates.x < fromCoordinates.x) {
                return Exit.SidePosition.Left;
            }
            if (atCoordinates.y > fromCoordinates.y) {
                return Exit.SidePosition.Bottom;
            }
            if (atCoordinates.y < fromCoordinates.y) {
                return Exit.SidePosition.Top;
            }

            return Exit.SidePosition.None;
        }
        
        public struct BinaryRequirements {
            public bool top;
            public bool bottom;
            public bool left;
            public bool right;

            public BinaryRequirements(bool top, bool bottom, bool left, bool right) {
                this.top = top;
                this.bottom = bottom;
                this.left = left;
                this.right = right;
            }
        }

    }
}