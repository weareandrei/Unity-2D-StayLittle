using UnityEngine;
using System.Collections.Generic;
using Dungeon.Chunk;
using Dungeon.Properties.Map.Type;
using UnityEditor.ShaderGraph.Internal;

namespace Dungeon.Generator.Stage {
    public static class ChunkGenerator {
        public static ChunkMap newChunkMap;
        public static ChunkMap GenerateChunks(string seed) {
            newChunkMap = new ChunkMap();

            List<string> chunksInUse = new List<string>(); // ID's
            List<ChunkMap.PossibleExit> possibleExits = new List<ChunkMap.PossibleExit>();

            while (chunksInUse.Count <= Consts.DungeonChunkCount ||
                   possibleExits.Count > 0) {
                foreach (ChunkMap.PossibleExit exit in possibleExits) {
                    Vector2 chunkCoordinates = GetNextChunkCoordinatesBasedOnExit(exit);
                    string thisCoordinatesContents =
                        newChunkMap.map.GetCell((int) chunkCoordinates[0], (int) chunkCoordinates[1]);
                    if (thisCoordinatesContents != "") {
                        possibleExits.Remove(exit);
                    }
                    
                    // If this cell is empty then we continue here :
                    
                    string newChunkId = SelectChunk(chunkCoordinates);
                    newChunkMap.PlaceCellOnMap(chunkCoordinates, newChunkId);
                    chunksInUse.Add(newChunkId);
                }
            }

            return newChunkMap;
        }

        private static string SelectChunk(Vector2 chunkCoordinatesOnGrid) {
            int posX = (int)chunkCoordinatesOnGrid[0];
            int posY = (int)chunkCoordinatesOnGrid[1];

            bool exitRight = false;
            bool exitLeft = false;
            bool exitTop = false;
            bool exitBottom = false;
            
            // Must check the chunks around
            //    must be connected to all exits

            ChunkLayout layoutRequirements = GetChunkRequirements(posX, posY);

            return "0";
        }

        private static Vector2 GetNextChunkCoordinatesBasedOnExit(ChunkMap.PossibleExit exit) {
            Vector2 newChunkCoordinates = new Vector2(exit.x, exit.y);
            switch (exit.position) {
                case ChunkMap.SidePosition.Top:
                    newChunkCoordinates.y++;
                    break;
                case ChunkMap.SidePosition.Bottom:
                    newChunkCoordinates.y--;
                    break;
                case ChunkMap.SidePosition.Left:
                    newChunkCoordinates.x--;
                    break;
                case ChunkMap.SidePosition.Right:
                    newChunkCoordinates.x++;
                    break;

                // default:
                //     throw new ArgumentOutOfRangeException();
            }

            return newChunkCoordinates;
        }

        private static ChunkLayout GetChunkRequirements(int x, int y) {
            ChunkLayout requirements;
            ChunkLayout foundLayout;
            // Must change the size of ChunkLayout
            
            foundLayout = 
                FindChunkByID(newChunkMap.map.GetCell(x-1, y));
            
            foundLayout = 
                FindChunkByID(newChunkMap.map.GetCell(x+1, y));
            
            foundLayout = 
                FindChunkByID(newChunkMap.map.GetCell(x, y-1));
            
            foundLayout = 
                FindChunkByID(newChunkMap.map.GetCell(x, y+1));

            return foundLayout;
        }

        private static ChunkLayout FindChunkByID(string id) {
            return new ChunkLayout(5);
        }
    }
}