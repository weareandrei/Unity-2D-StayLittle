using UnityEngine;
using System.Collections.Generic;
using Dungeon.Properties.MapUtilities;
using Dungeon.Room;

namespace Dungeon.Properties {
    
    public class DungeonData {
        private const int ChunkSize = DungeonConsts.defaultChunkSize;

        // Map consists of ID's
        public DungeonMap map = new DungeonMap();
        public int direction = 1;
        public List<RoomInstance> roomsInDungeon = new List<RoomInstance>();

        public List<RoomInstance> roomsAvailable;
        
        public void InsertEntrance() {
            int posX = 0;
            int posY = (ChunkSize / 2) - 1;
            // Entrance will be in the center Vertically and in
            //  the start of Chunk Horizontally
            foreach (RoomInstance room in roomsAvailable) {
                if (room.type == RoomType.Entrance) {
                    // Create an array of all room instances
                    //  and pick the one that correspods to seed
            
                    // For now just pick the first one
                    roomsInDungeon.Add(room);
                }
            }
        }
        
        public Vector2 FindCurrentChunkUsingCoordinate(int x, int y) {
            int chunkPosX = 0;
            int chunkPosY = 0;
            
            if (x <= ChunkSize - 1 ||
                y <= ChunkSize - 1) {
                return new Vector2(chunkPosX, chunkPosY);
            }

            chunkPosX = x / (x % ChunkSize);
            chunkPosY = x / (y % ChunkSize);

            return new Vector2(chunkPosX, chunkPosY);
        }

        public int[,] GetNewChunkExitRequirements(Vector2 chunkPos) {
            int chunkPosX = (int)chunkPos[0];
            int chunkPosY = (int)chunkPos[1];

            int[,] exitsLocated = new int[ChunkSize,ChunkSize];
            
            // Bottom Left - Bottom Right
            exitsLocated = BottomLeft_BottomRight(exitsLocated, chunkPosX, chunkPosY);
            exitsLocated = BottomLeft_TopLeft(exitsLocated, chunkPosX, chunkPosY);
            exitsLocated = TopLeft_TopRight(exitsLocated, chunkPosX, chunkPosY);
            exitsLocated = BottomRight_TopRight(exitsLocated, chunkPosX, chunkPosY);

            return exitsLocated;
        }

        private int[,] BottomLeft_BottomRight(int[,] exitsLocated, int chunkPosX, int chunkPosY) {
            for (int currentX = chunkPosX * ChunkSize;
                 currentX < (chunkPosX+1) * ChunkSize; 
                 currentX++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, currentX, chunkPosY * ChunkSize - 1, 
                    chunkPosX, chunkPosY);            
            }
            
            return exitsLocated;
        }

        private int[,] BottomLeft_TopLeft(int[,] exitsLocated, int chunkPosX, int chunkPosY) {
            for (int currentY = chunkPosY * ChunkSize - 1;
                 currentY < (chunkPosY + 1) * ChunkSize + 1;
                 currentY++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, chunkPosX * ChunkSize - 1, currentY, 
                    chunkPosX, chunkPosY);
            }
            
            return exitsLocated;
        }
        
        private int[,] TopLeft_TopRight(int[,] exitsLocated, int chunkPosX, int chunkPosY) {
            for (int currentX = chunkPosX * ChunkSize;
                 currentX < (chunkPosX+1) * ChunkSize; 
                 currentX++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, currentX, (chunkPosY + 1) * ChunkSize - 1, 
                    chunkPosX, chunkPosY);
                
            }
            
            return exitsLocated;
        }
        
        private int[,] BottomRight_TopRight(int[,] exitsLocated, int chunkPosX, int chunkPosY) {
            for (int currentY = chunkPosY * ChunkSize - 1;
                 currentY < (chunkPosY + 1) * ChunkSize + 1;
                 currentY++) 
            {
                exitsLocated = ReviewThisPositionForExit(
                    exitsLocated, (chunkPosX + 1) * ChunkSize - 1, currentY, 
                    chunkPosX, chunkPosY);             
            }
            
            return exitsLocated;
        }

        private int[,] ReviewThisPositionForExit(int[,] exitsLocated, int currentX, int currentY,
                                                 int chunkPosX, int chunkPosY) {
            if (IsCoordinateValid(currentX, currentY) != true) {
                return exitsLocated;
            }

            RoomInstance roomAtThisPosition = GetRoomAtPosition(currentX, currentY);
            if (roomAtThisPosition.type == RoomType.Entrance) { 
                exitsLocated = TransferCoordinatesFromMapToExitSchema(
                    exitsLocated, (chunkPosX + 1) * ChunkSize - 1, currentY, 
                    chunkPosX, chunkPosY);
            }

            return exitsLocated;
        }

        private RoomInstance GetRoomAtPosition(int currentX, int currentY) {
            string roomIdAtThisPosition = map.GetRoom(currentX, currentY);
            return roomsAvailable.Find(
                room => room.roomInstanceID == roomIdAtThisPosition);
        }

        private int[,] TransferCoordinatesFromMapToExitSchema(int[,] exitsLocated, int currentX, int currentY,
                                                int chunkPosX, int chunkPosY) {
            int exitCoordX = currentX - (chunkPosX * ChunkSize - 1);
            int exitCoordY = currentY - (chunkPosY * ChunkSize - 1);
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

