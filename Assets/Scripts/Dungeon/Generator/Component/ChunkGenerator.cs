using System;
using UnityEngine;
using System.Collections.Generic;
using Dungeon.Model;
using HoneyGrid2D;

namespace Dungeon.Generator {
    public static class ChunkGenerator {
        private static ChunkMap _newChunkMap;
        public static List<Chunk> chunkLayoutsAvailable;

        private static Exit.SidePosition _exitDirection;
        private static Vector2Int _furthestExit = new Vector2Int(0,0);

        public static ChunkMap GenerateChunks() {
            _newChunkMap = new ChunkMap();
            _exitDirection = DungeonGenerator.exitDirection;

            List<string> chunksInUse = new List<string>(); // ID's
            List<Exit.PossibleExit> possibleExits = new List<Exit.PossibleExit>();
            // Here we use Abstract coordinate system. So Top is at the bottom (in Debug window)

            possibleExits.Add(new Exit.PossibleExit(1, 0, Exit.SidePosition.Left));

            while (chunksInUse.Count < Consts.Get<int>("DungeonChunkCount")) {
                // _newChunkMap.map.DisplayGrid(); 
                Exit.PossibleExit exit = possibleExits[0];
                Vector2Int newChunkCoordinates = GetNextChunkCoordinatesBasedOnExit(exit);

                string newChunkCoordinatesContents = "none";
                try {
                    newChunkCoordinatesContents =
                        _newChunkMap.map.GetCell(newChunkCoordinates.x, newChunkCoordinates.y);
                } catch (IndexOutOfRangeException e) { }
                
                if (newChunkCoordinatesContents != "") {
                    possibleExits.Remove(exit);
                    continue;
                }
                
                // If that cell is empty then we continue here :
                
                string newChunkId = SelectChunk(exit, newChunkCoordinates);
                if (newChunkId == "not found") {
                    possibleExits.Remove(exit);
                    continue;
                    
                    // todo: there is a bug. When E is at the angle of the ChunkLayout we can
                    // todo:    use it both for left and top for example.
                    // todo:       But we will remove the exit after the took 1 of the sides
                }
                _newChunkMap.PlaceCellOnMap(newChunkCoordinates, newChunkId);
                chunksInUse.Add(newChunkId);

                Chunk newChunk = FindChunkLayoutByID(newChunkId);
                possibleExits.AddRange(
                    FindPossibleExits(newChunk.rooms, newChunkCoordinates.x, newChunkCoordinates.y));
            }

            // _newChunkMap.map.DisplayGrid();
            return _newChunkMap;
        }

        public static Chunk FindChunkLayoutByID(string id) {
            if (id == "") {
                throw new ArgumentException("Chunk ID can't be empty");
            }
            return chunkLayoutsAvailable.FindAll( layout => layout.ID == id)[0];
        }

        private static List<Exit.PossibleExit> FindPossibleExits(Grid2DString addedLayout, int coordX, int coordY) {
            List<Exit.PossibleExit> possibleExitsFound = new List<Exit.PossibleExit>();
            
            // Bottom
            for (int x = 0; x <= Consts.Get<int>("ChunkSize")-1; x++) {
                if (addedLayout.GetCell(x, Consts.Get<int>("ChunkSize")-1) == "E") {
                    possibleExitsFound.Add(
                        new Exit.PossibleExit(coordX,coordY,Exit.SidePosition.Bottom));
                    break;
                }
            }
            
            // Top
            for (int x = 0; x <= Consts.Get<int>("ChunkSize")-1; x++) {
                if (addedLayout.GetCell(x, 0) == "E") {
                    possibleExitsFound.Add(
                        new Exit.PossibleExit(coordX,coordY,Exit.SidePosition.Top));
                    break;
                }
            }
            
            // Left
            for (int y = 0; y <= Consts.Get<int>("ChunkSize") - 1; y++) {
                if (addedLayout.GetCell(0, y) == "E") {
                    possibleExitsFound.Add(
                        new Exit.PossibleExit(coordX,coordY,Exit.SidePosition.Left));
                    break;
                }
            }

            // Right
            for (int y = 0; y <= Consts.Get<int>("ChunkSize")-1; y++) {
                if (addedLayout.GetCell(Consts.Get<int>("ChunkSize")-1, y) == "E") {
                    possibleExitsFound.Add(
                        new Exit.PossibleExit(coordX,coordY,Exit.SidePosition.Right));
                    break;
                }
            }
            
            
            return possibleExitsFound;
        }

        private static string SelectChunk(Exit.PossibleExit exitFrom,Vector2Int chunkCoordinatesOnGrid) {
            int posX = chunkCoordinatesOnGrid.x;
            int posY = chunkCoordinatesOnGrid.y;

            Grid2DString layoutRequirements = GetLayoutRequirements(exitFrom, posX, posY);
            List<string> foundLayouts = FindSimilarChunkLayout(layoutRequirements);
            if (foundLayouts.Count == 0) {
                return "not found";
            }
            // todo : next line has a timeout if we don't find the Layouts appropriate
            return foundLayouts[DungeonGenerator.UseSeed(foundLayouts.Count-1)];
        }

        private static Vector2Int GetNextChunkCoordinatesBasedOnExit(Exit.PossibleExit exit) {
            Vector2Int newChunkCoordinates = new Vector2Int(exit.x, exit.y);
            switch (exit.position) {
                case Exit.SidePosition.Top:
                    newChunkCoordinates.y++;
                    break;
                case Exit.SidePosition.Bottom:
                    newChunkCoordinates.y--;
                    break;
                case Exit.SidePosition.Left:
                    newChunkCoordinates.x++;
                    break;
                case Exit.SidePosition.Right:
                    newChunkCoordinates.x--;
                    break;
            }

            return newChunkCoordinates;
        }

        // Notion Documentation
        // https://www.notion.so/Chunk-0f779170060245a0a4deae866a623904?pvs=4#5f4e2e18d60646fe813fa29b857aea90
        private static Grid2DString GetLayoutRequirements(Exit.PossibleExit exitFrom, int x, int y) {
            Grid2DString requirements = new Grid2DString(Consts.Get<int>("ChunkSize"));
            Grid2DString attachedChunk;

            switch (exitFrom.position) {
                case Exit.SidePosition.Top:
                    
                    try {
                        attachedChunk = FindChunkLayoutByID(_newChunkMap.map.GetCell(x, y-1)).rooms;
                        requirements = UpdateLayoutRequirements(requirements, attachedChunk, "bottom"); 
                    }
                    catch (IndexOutOfRangeException e) { /*Debug.Log(e.Message);*/ }
                    catch (ArgumentException e) { /*Debug.Log(e.Message);*/ }
                    break;
                
                case Exit.SidePosition.Bottom:

                    try {
                        attachedChunk = FindChunkLayoutByID(_newChunkMap.map.GetCell(x, y+1)).rooms;
                        requirements = UpdateLayoutRequirements(requirements, attachedChunk, "top");  
                    }
                    catch (IndexOutOfRangeException e) { /*Debug.Log(e.Message);*/ }
                    catch (ArgumentException e) { /*Debug.Log(e.Message);*/ }
                    break;
                
                case Exit.SidePosition.Left:
                    
                    try {
                        attachedChunk = FindChunkLayoutByID(_newChunkMap.map.GetCell(x-1, y)).rooms;
                        requirements = UpdateLayoutRequirements(requirements, attachedChunk, "right"); 
                    }
                    catch (IndexOutOfRangeException e) { /*Debug.Log(e.Message);*/ }
                    catch (ArgumentException e) { /*Debug.Log(e.Message);*/ }
                    break;
                
                case Exit.SidePosition.Right:

                    try {
                        attachedChunk = FindChunkLayoutByID(_newChunkMap.map.GetCell(x +1, y)).rooms;
                        requirements = UpdateLayoutRequirements(requirements, attachedChunk, "left");
                    }
                    catch (IndexOutOfRangeException e) { /*Debug.Log(e.Message);*/ }
                    catch (ArgumentException e) { /*Debug.Log(e.Message);*/ }
                    break;

            }

            requirements = ConsiderDungeonExit(requirements, new Vector2Int(x, y));
            
            return requirements;
        }

        private static Grid2DString ConsiderDungeonExit(Grid2DString requirements, Vector2Int requirementsCoordinates) {
            if (_exitDirection == Exit.SidePosition.Right && requirementsCoordinates.x > _furthestExit.x) {
                _furthestExit.x = requirementsCoordinates.x;
                for (int y = 0; y < Consts.Get<int>("ChunkSize")-1; y++) {
                    requirements.UpdateCell(Consts.Get<int>("ChunkSize")-1, y, "P");
                }
            }
            if (_exitDirection == Exit.SidePosition.Left && requirementsCoordinates.x < _furthestExit.x) {
                _furthestExit.x = requirementsCoordinates.x;
                for (int y = 0; y < Consts.Get<int>("ChunkSize")-1; y++) {
                    requirements.UpdateCell(0, y, "P");
                }
            }

            return requirements;
        }

        // We assume that we need all of those exits.
        //  But maybe requiring not all but a half ot the exits would be better to find the layout more easily
        private static List<string> FindSimilarChunkLayout(Grid2DString layout) {
            List<string> chunkLayouts = new List<string>();
            
            foreach (Chunk assessedChunkLayout in chunkLayoutsAvailable) {
                // Must check if it includes all the exits
                bool isSimilar = true;
                int potentialRooms = 0;
                int potentialRoomsSatisfied = 0;
                
                for (int y = 0; y <= layout.Size-1; y++) {
                    for (int x = 0; x <= layout.Size-1; x++) {
                        if (layout.GetCell(x, y) == "E") {
                            if (assessedChunkLayout.rooms.GetCell(x,y) != "E") {
                                isSimilar = false;
                            }
                        }
                        // BUG : only works if we satisfy 1 Possible Room.
                        if (layout.GetCell(x, y) == "P") {
                            potentialRooms++;
                            if (assessedChunkLayout.rooms.GetCell(x,y) != "E" || 
                                assessedChunkLayout.rooms.GetCell(x,y) != "R") {
                                potentialRoomsSatisfied++;
                            }
                        }
                    }
                }

                // If we did not satisfy any of the Potential Rooms
                if (potentialRooms > 0) {
                    if (potentialRoomsSatisfied == 0) {
                        isSimilar = false;
                    }
                }

                if (isSimilar) {
                    chunkLayouts.Add(assessedChunkLayout.ID);
                }
            }
            
            return chunkLayouts;
        }

        private static Grid2DString UpdateLayoutRequirements(Grid2DString requirements, Grid2DString attachedChunk, string side) {
            switch (side) {
                case "left": {
                    for (int y = 0; y < Consts.Get<int>("ChunkSize")-1; y++) {
                        if (attachedChunk.GetCell(Consts.Get<int>("ChunkSize")-1, y) == "E") {
                            requirements.UpdateCell(0, y, "E");
                        }
                    }

                    break;
                }
                case "right": {
                    for (int y = 0; y < Consts.Get<int>("ChunkSize")-1; y++) {
                        if (attachedChunk.GetCell(0, y) == "E") {
                            requirements.UpdateCell(Consts.Get<int>("ChunkSize")-1, y, "E");
                        }
                    }

                    break;
                }
                case "top": {
                    for (int x = 0; x < Consts.Get<int>("ChunkSize")-1; x++) {
                        if (attachedChunk.GetCell(x, 0) == "E") {
                            requirements.UpdateCell(x, Consts.Get<int>("ChunkSize")-1, "E");
                        }
                    }

                    break;
                }
                case "bottom": {
                    for (int x = 0; x < Consts.Get<int>("ChunkSize")-1; x++) {
                        if (attachedChunk.GetCell(x, Consts.Get<int>("ChunkSize")-1) == "E") {
                            requirements.UpdateCell(x, 0, "E");
                        }
                    }

                    break;
                }
            }

            return requirements;
        }

        private static Chunk GetEntranceLayout() {
            foreach (Chunk layout in chunkLayoutsAvailable) {
                if (layout.chunkType == ChunkType.Entrance) {
                    return layout;
                }
            }

            return null;
        }
    }
}