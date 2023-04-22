using System.Runtime.CompilerServices;
using DataPersistence;
using Dungeon.Properties;
using Dungeon.Properties.Map.Type;
using UnityEngine;
    
namespace Dungeon.Renderer {
    public static class DungeonRenderer {

        private static int _spaceBetweenDungeons = 12; 
        
        // Based on this we can know if this is a Right or Left side Dungeon
        private static Vector2Int _dungeonOrigin; 
        
        // Here we Render only 1 specific Dungeon
        public static void RenderDungeon (DungeonData dungeonData, DungeonMapData dungeonMapData) {
            _dungeonOrigin = CalcDungeonOrigin(dungeonData.coordinates);
            RenderRooms(dungeonMapData.roomMap);
        }

        private static void RenderRooms(RoomMap roomMap) {
            for (int y = 0; y < roomMap.map.getYSize() - 1; y++) {
                for (int x = 0; x < roomMap.map.getXSize() - 1; x++) {
                    Vector2Int cellPosFromOrigin = new Vector2Int(_dungeonOrigin.x + (x*_spaceBetweenDungeons), _dungeonOrigin.y + (y*_spaceBetweenDungeons));
                    string roomId = roomMap.map.GetCell(x, y);
                    if (roomId != "") {
                        RenderRoomAtCoordinates(cellPosFromOrigin, roomMap.map.GetCell(x,y));
                    }
                }
            }
        }

        private static void RenderRoomAtCoordinates(Vector2Int coordinates, string roomId) {
            GameObject roomPrefab = Generator.Generator.GetRoomPrefabFromID(roomId);
            if (roomPrefab == null) {
                return;
            }
            Vector3 position3D = new Vector3(coordinates.x, coordinates.y, 0);
            Debug.Log("Instantiate Room");
            GameObject renderedPrefab = GameObject.Instantiate(roomPrefab, position3D, Quaternion.identity);
            Debug.Log(" + Instantiated Room");

            // Set the position of the rendered prefab
            // renderedPrefab.transform.position = coordinates;
        }

        private static Vector2Int CalcDungeonOrigin(Vector2Int coordinates) {
            // Should calculate based on Right or Left side
            return new Vector2Int(0, 0);
        }
    }
}
