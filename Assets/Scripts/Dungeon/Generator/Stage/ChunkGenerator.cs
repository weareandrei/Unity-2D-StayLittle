using UnityEngine;
using System.Collections.Generic;
using Dungeon.Chunk;
using Dungeon.Properties.Map.Type;
using Grid2DEditor;

namespace Dungeon.Generator.Stage {
    public static class ChunkGenerator {
        
        public static ChunkMap newChunkMap;
        public static List<ChunkLayout> chunkLayoutsAvailable;

        public static ChunkMap GenerateChunks(string seed) {
            newChunkMap = new ChunkMap();

            List<string> chunksInUse = new List<string>(); // ID's
            List<ChunkMap.PossibleExit> possibleExits = new List<ChunkMap.PossibleExit>();

            while (chunksInUse.Count <= Consts.DungeonChunkCount || possibleExits.Count > 0) {
                foreach (ChunkMap.PossibleExit exit in possibleExits) { 
                    Vector2Int newChunkCoordinates = GetNextChunkCoordinatesBasedOnExit(exit);
                    string newChunkCoordinatesContents =
                        newChunkMap.map.GetCell(newChunkCoordinates.x, newChunkCoordinates.y);
                    if (newChunkCoordinatesContents != "") {
                        possibleExits.Remove(exit);
                    }
                    
                    // If this cell is empty then we continue here :
                    
                    string newChunkId = SelectChunk(newChunkCoordinates);
                    if (newChunkId == "not found") {
                        Debug.Log("Chunk not found!");
                    }
                    newChunkMap.PlaceCellOnMap(newChunkCoordinates, newChunkId);
                    chunksInUse.Add(newChunkId);
                }
            }

            return newChunkMap;
        }

        private static string SelectChunk(Vector2Int chunkCoordinatesOnGrid) {
            int posX = chunkCoordinatesOnGrid.x;
            int posY = chunkCoordinatesOnGrid.y;

            // Must check the chunks around
            //    must be connected to all exits

            Grid2D layoutRequirements = GetLayoutRequirements(posX, posY);
            return FindSimilarChunkLayout(layoutRequirements);
        }

        private static Vector2Int GetNextChunkCoordinatesBasedOnExit(ChunkMap.PossibleExit exit) {
            Vector2Int newChunkCoordinates = new Vector2Int(exit.x, exit.y);
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

        private static Grid2D GetLayoutRequirements(int x, int y) {
            Grid2D requirements = new Grid2D(Consts.ChunkSize);
            Grid2D foundChunkLayout;
            
            // Go through each cell on the right column and copy exits to left column in requirements grid.
            foundChunkLayout = FindChunkLayoutByID(newChunkMap.map.GetCell(x-1, y));
            requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "left");
            
            foundChunkLayout = FindChunkLayoutByID(newChunkMap.map.GetCell(x+1, y));
            requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "right");

            foundChunkLayout = FindChunkLayoutByID(newChunkMap.map.GetCell(x, y-1));
            requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "bottom");

            foundChunkLayout = FindChunkLayoutByID(newChunkMap.map.GetCell(x, y+1));
            requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "top");

            return requirements;
        }

        private static Grid2D FindChunkLayoutByID(string id) {
            return new Grid2D(5);
        }

        // We assume that we need all of those exits.
        //  But maybe requiring not all but a half ot the exits would be better to find the layout more easily
        private static string FindSimilarChunkLayout(Grid2D layout) {
            foreach (ChunkLayout chunkLayout in chunkLayoutsAvailable) {
                // Must check if it includes all the exits
                bool isSimilar = true;
                for (int y = 0; y < layout.Size-1; y++) {
                    for (int x = 0; x < layout.Size-1; x++) {
                        if (layout.GetCell(x, y) == "E") {
                            if (chunkLayout.rooms.GetCell(x,y) != "E") {
                                isSimilar = false;
                            }
                        }
                    }
                }

                if (isSimilar) {
                    return chunkLayout.ID;
                }
            }
            return "not found";
        }

        private static Grid2D UpdateLayoutRequirements(Grid2D requirements, Grid2D attachedChunk, string side) {
            switch (side) {
                case "left": {
                    for (int y = 0; y < Consts.ChunkSize-1; y++) {
                        if (attachedChunk.GetCell(Consts.ChunkSize-1, y) == "E") {
                            requirements.UpdateCell(0, y, "E");
                        }
                    }

                    break;
                }
                case "right": {
                    for (int y = 0; y < Consts.ChunkSize-1; y++) {
                        if (attachedChunk.GetCell(0, y) == "E") {
                            requirements.UpdateCell(Consts.ChunkSize-1, y, "E");
                        }
                    }

                    break;
                }
                case "top": {
                    for (int x = 0; x < Consts.ChunkSize-1; x++) {
                        if (attachedChunk.GetCell(x, 0) == "E") {
                            requirements.UpdateCell(x, Consts.ChunkSize-1, "E");
                        }
                    }

                    break;
                }
                case "bottom": {
                    for (int x = 0; x < Consts.ChunkSize-1; x++) {
                        if (attachedChunk.GetCell(x, Consts.ChunkSize-1) == "E") {
                            requirements.UpdateCell(x, 0, "E");
                        }
                    }

                    break;
                }
            }

            return requirements;
        }
    }
}