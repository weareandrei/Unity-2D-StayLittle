using UnityEngine;
using System.Collections.Generic;
using Dungeon.Properties.MapUtilities;
using Dungeon.Room;

namespace Dungeon.Properties {
    
    public class DungeonData {
        private const int QuadrantSize = DungeonConsts.defaultQuadrantSize;

        // Map consists of ID's
        public DungeonMap map = new DungeonMap();
        public int direction = 1;
        public List<RoomInstance> roomsInDungeon = new List<RoomInstance>();

        public List<RoomInstance> roomsAvailable;
        
        public void InsertEntrance() {
            int posX = 0;
            int posY = (QuadrantSize / 2) - 1;
            // Entrance will be in the center Vertically and in
            //  the start of Quadrant Horizontally
            foreach (RoomInstance room in roomsAvailable) {
                if (room.type == RoomType.Entrance) {
                    // Create an array of all room instances
                    //  and pick the one that correspods to seed
            
                    // For now just pick the first one
                    roomsInDungeon.Add(room);
                }
            }
        }
        
        public Vector2 FindCurrentQuadrantUsingCoordinate(int x, int y) {
            int quadrantPosX = 0;
            int quadrantPosY = 0;
            
            if (x <= QuadrantSize - 1 ||
                y <= QuadrantSize - 1) {
                return new Vector2(quadrantPosX, quadrantPosY);
            }

            quadrantPosX = x / (x % QuadrantSize);
            quadrantPosY = x / (y % QuadrantSize);

            return new Vector2(quadrantPosX, quadrantPosY);
        }

        public int[,] GetNewQuadrantExitRequirements(Vector2 quadrantPos) {
            int quadrantPosX = (int)quadrantPos[0];
            int quadrantPosY = (int)quadrantPos[1];

            int[,] exitsLocated = new int[QuadrantSize,QuadrantSize];
            
            // Bottom Left - Bottom Right
            exitsLocated = BottomLeft_BottomRight(exitsLocated, quadrantPosX, quadrantPosY);
            exitsLocated = BottomLeft_TopLeft(exitsLocated, quadrantPosX, quadrantPosY);
            exitsLocated = TopLeft_TopRight(exitsLocated, quadrantPosX, quadrantPosY);
            exitsLocated = BottomRight_TopRight(exitsLocated, quadrantPosX, quadrantPosY);

            return exitsLocated;
        }

        private int[,] BottomLeft_BottomRight(int[,] exitsLocated, int quadrantPosX, int quadrantPosY) {
            for (int currentX = quadrantPosX * QuadrantSize;
                 currentX < (quadrantPosX+1) * QuadrantSize; 
                 currentX++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, currentX, quadrantPosY * QuadrantSize - 1, 
                    quadrantPosX, quadrantPosY);            
            }
            
            return exitsLocated;
        }

        private int[,] BottomLeft_TopLeft(int[,] exitsLocated, int quadrantPosX, int quadrantPosY) {
            for (int currentY = quadrantPosY * QuadrantSize - 1;
                 currentY < (quadrantPosY + 1) * QuadrantSize + 1;
                 currentY++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, quadrantPosX * QuadrantSize - 1, currentY, 
                    quadrantPosX, quadrantPosY);
            }
            
            return exitsLocated;
        }
        
        private int[,] TopLeft_TopRight(int[,] exitsLocated, int quadrantPosX, int quadrantPosY) {
            for (int currentX = quadrantPosX * QuadrantSize;
                 currentX < (quadrantPosX+1) * QuadrantSize; 
                 currentX++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, currentX, (quadrantPosY + 1) * QuadrantSize - 1, 
                    quadrantPosX, quadrantPosY);
                
            }
            
            return exitsLocated;
        }
        
        private int[,] BottomRight_TopRight(int[,] exitsLocated, int quadrantPosX, int quadrantPosY) {
            for (int currentY = quadrantPosY * QuadrantSize - 1;
                 currentY < (quadrantPosY + 1) * QuadrantSize + 1;
                 currentY++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, (quadrantPosX + 1) * QuadrantSize - 1, currentY, 
                    quadrantPosX, quadrantPosY);             
            }
            
            return exitsLocated;
        }

        private int[,] ReviewThisPositionForExit(int[,] exitsLocated, int currentX, int currentY,
                                                 int quadrantPosX, int quadrantPosY) {
            if (IsCoordinateValid(currentX, currentY) != true) {
                return exitsLocated;
            }

            RoomInstance roomAtThisPosition = GetRoomAtPosition(currentX, currentY);
            if (roomAtThisPosition.type == RoomType.Entrance) { 
                exitsLocated = TransferCoordinatesFromMapToExitSchema(
                    exitsLocated, (quadrantPosX + 1) * QuadrantSize - 1, currentY, 
                    quadrantPosX, quadrantPosY);
            }

            return exitsLocated;
        }

        private RoomInstance GetRoomAtPosition(int currentX, int currentY) {
            string roomIdAtThisPosition = map.GetRoom(currentX, currentY);
            return roomsAvailable.Find(
                room => room.roomInstanceID == roomIdAtThisPosition);
        }

        private int[,] TransferCoordinatesFromMapToExitSchema(int[,] exitsLocated, int currentX, int currentY,
                                                int quadrantPosX, int quadrantPosY) {
            int exitCoordX = currentX - (quadrantPosX * QuadrantSize - 1);
            int exitCoordY = currentY - (quadrantPosY * QuadrantSize - 1);
            exitsLocated[exitCoordX, exitCoordY] = 1;
            return exitsLocated;
        }

        private bool IsCoordinateValid(int x, int y) {
            if (x < 0 || y < 0) {
                return false;
            }

            if ((map.columns.Count - 1) > x ||
                map.columns[0].column.Count -1 > y) {
                return false;
            }

            return true;
        }
    }
}

