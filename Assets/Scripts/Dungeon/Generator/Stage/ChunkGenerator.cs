using System;
using UnityEngine;
using System.Collections.Generic;
using Dungeon.Chunk;
using Dungeon.Properties.Map.Type;
using Grid2DEditor;

namespace Dungeon.Generator.Stage {
    public static class ChunkGenerator {
        private static ChunkMap _newChunkMap;
        public static List<ChunkLayout> chunkLayoutsAvailable;
        public static string seed;

        public static ChunkMap GenerateChunks() {
            _newChunkMap = new ChunkMap();

            List<string> chunksInUse = new List<string>(); // ID's
            List<ChunkMap.PossibleExit> possibleExits = new List<ChunkMap.PossibleExit>();
            
            // Place Entrance
            ChunkLayout entranceLayout = GetEntranceLayout();
            _newChunkMap.PlaceCellOnMap(new Vector2Int(0,0), entranceLayout.ID);
            
            chunksInUse.Add(entranceLayout.ID);
            possibleExits.AddRange(
                FindPossibleExits(entranceLayout.rooms, 0, 0));
            // todo: remember that 1 exit here is actually an entrance from the elevator

            while (chunksInUse.Count <= Consts.DungeonChunkCount) {
                _newChunkMap.map.DisplayGrid(); 
                ChunkMap.PossibleExit exit = possibleExits[0];
                Vector2Int newChunkCoordinates = GetNextChunkCoordinatesBasedOnExit(exit);

                string newChunkCoordinatesContents = "none";
                try {
                    newChunkCoordinatesContents =
                        _newChunkMap.map.GetCell(newChunkCoordinates.x, newChunkCoordinates.y);
                }
                catch (IndexOutOfRangeException e) {
                    // Console.WriteLine(e);
                    Debug.Log(e.Message);
                }
                
                if (newChunkCoordinatesContents != "") {
                    possibleExits.Remove(exit);
                    continue;
                }
                
                // If this cell is empty then we continue here :
                
                string newChunkId = SelectChunk(newChunkCoordinates);
                if (newChunkId == "not found") {
                    Debug.Log("Chunk not found!");
                }
                _newChunkMap.PlaceCellOnMap(newChunkCoordinates, newChunkId);
                chunksInUse.Add(newChunkId);

                ChunkLayout newChunkLayout = FindChunkLayoutByID(newChunkId);
                possibleExits.AddRange(
                    FindPossibleExits(newChunkLayout.rooms, newChunkCoordinates.x, newChunkCoordinates.y));

                Debug.Log("Here");
            }

            _newChunkMap.map.DisplayGrid();
            return _newChunkMap;
        }

        private static ChunkLayout FindChunkLayoutByID(string id) {
            if (id == "") {
                throw new ArgumentException("Chunk ID can't be empty");
            }
            return chunkLayoutsAvailable.FindAll( layout => layout.ID == id)[0];
        }

        private static List<ChunkMap.PossibleExit> FindPossibleExits(Grid2D addedLayout, int coordX, int coordY) {
            List<ChunkMap.PossibleExit> possibleExitsFound = new List<ChunkMap.PossibleExit>();
            
            // Top
            for (int x = 0; x < Consts.ChunkSize-1; x++) {
                if (addedLayout.GetCell(x, Consts.ChunkSize-1) == "E") {
                    possibleExitsFound.Add(
                        new ChunkMap.PossibleExit(coordX,coordY,ChunkMap.SidePosition.Top));
                    break;
                }
            }
            
            // Bottom
            for (int x = 0; x < Consts.ChunkSize-1; x++) {
                if (addedLayout.GetCell(x, 0) == "E") {
                    possibleExitsFound.Add(
                        new ChunkMap.PossibleExit(coordX,coordY,ChunkMap.SidePosition.Bottom));
                    break;
                }
            }
            
            // Left
            for (int y = 0; y < Consts.ChunkSize - 1; y++) {
                if (addedLayout.GetCell(0, y) == "E") {
                    possibleExitsFound.Add(
                        new ChunkMap.PossibleExit(coordX,coordY,ChunkMap.SidePosition.Left));
                    break;
                }
            }

            // Right
            for (int y = 0; y < Consts.ChunkSize-1; y++) {
                if (addedLayout.GetCell(Consts.ChunkSize-1, y) == "E") {
                    possibleExitsFound.Add(
                        new ChunkMap.PossibleExit(coordX,coordY,ChunkMap.SidePosition.Right));
                    break;
                }
            }
            
            
            return possibleExitsFound;
        }

        private static string SelectChunk(Vector2Int chunkCoordinatesOnGrid) {
            int posX = chunkCoordinatesOnGrid.x;
            int posY = chunkCoordinatesOnGrid.y;

            // Must check the chunks around
            //    must be connected to all exits

            Grid2D layoutRequirements = GetLayoutRequirements(posX, posY);
            List<string> foundLayouts = FindSimilarChunkLayout(layoutRequirements);
            return foundLayouts[UseSeed(foundLayouts.Count)];
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
            
            try {
                foundChunkLayout = FindChunkLayoutByID(_newChunkMap.map.GetCell(x - 1, y)).rooms;
                requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "left");
            }
            catch (IndexOutOfRangeException e) { /*Debug.Log(e.Message);*/ }
            catch (ArgumentException e) { /*Debug.Log(e.Message);*/ }

            try {
                foundChunkLayout = FindChunkLayoutByID(_newChunkMap.map.GetCell(x+1, y)).rooms;
                requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "right"); 
            }
            catch (IndexOutOfRangeException e) { Debug.Log(e.Message); }
            catch (ArgumentException e) { Debug.Log(e.Message); }
            
            try {
                foundChunkLayout = FindChunkLayoutByID(_newChunkMap.map.GetCell(x, y-1)).rooms;
                requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "bottom");   
            }
            catch (IndexOutOfRangeException e) { Debug.Log(e.Message); }
            catch (ArgumentException e) { Debug.Log(e.Message); }
            
            try {
                foundChunkLayout = FindChunkLayoutByID(_newChunkMap.map.GetCell(x, y+1)).rooms;
                requirements = UpdateLayoutRequirements(requirements, foundChunkLayout, "top");  
            }
            catch (IndexOutOfRangeException e) { Debug.Log(e.Message); }
            catch (ArgumentException e) { Debug.Log(e.Message); }
            
            return requirements;
        }

        // We assume that we need all of those exits.
        //  But maybe requiring not all but a half ot the exits would be better to find the layout more easily
        private static List<string> FindSimilarChunkLayout(Grid2D layout) {
            List<string> chunkLayouts = new List<string>();
            
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
                    chunkLayouts.Add(chunkLayout.ID);
                }
            }
            
            return chunkLayouts;
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

        private static ChunkLayout GetEntranceLayout() {
            ChunkLayout returnLayout = new ChunkLayout(); // todo: remove later 
            foreach (ChunkLayout layout in chunkLayoutsAvailable) {
                if (layout.chunkType == ChunkType.Entrance) {
                    return layout;
                }
            }

            return returnLayout; // todo: remove later 
        }

        private static int UseSeed(int choiceCount) {
            int seedNumber = (int)seed[0];
            seed = seed.Substring(1, seed.Length - 1) + seedNumber;

            while (choiceCount <= seedNumber) {
                seedNumber = seedNumber - choiceCount;
                if (seedNumber < 0) {
                    seedNumber = 0;
                }
            }
            
            return seedNumber;
        }
    }
}