using System.Collections.Generic;
using Content;
using Dungeon.Data;
using Dungeon.Model;
using HoneyGrid2D;
using UnityEngine;
    
namespace Dungeon.Renderer {
    public static class DungeonRenderer {

        private static int _spaceBetweenDungeons = 12;
        
        private static RoomMap _roomMap;
        private static ContentsMap _contentsMap;
        
        // Based on this we can know if this is a Right or Left side Dungeon
        private static Vector2 _dungeonOrigin; 
        
        // Here we Render only 1 specific Dungeon
        public static void RenderDungeon(DungeonData dungeonData, DungeonMapData dungeonMapData) {
            _roomMap = dungeonMapData.roomMap;
            _contentsMap = dungeonMapData.contentsMap;

            _dungeonOrigin = CalcDungeonOrigin(dungeonData);

            GameObject dungeonParent = new GameObject("Dungeon");
            DungeonDataContainer newDataContainer = dungeonParent.AddComponent<DungeonDataContainer>();
            newDataContainer.data = dungeonData;

            RenderRooms(_roomMap, dungeonParent);
            // RenderContents(_contentsMap);
        }


        private static void RenderRooms(RoomMap roomMap, GameObject dungeonParent) {
            for (int y = 0; y < roomMap.map.getYSize(); y++) {
                for (int x = 0; x < roomMap.map.getXSize(); x++) {
                    Vector2 cellPosFromOrigin = DetermineCellPosition(new Vector2Int(x,y));
                    string roomId = roomMap.map.GetCellActual(roomMap.map.getXSize() - 1 - x, y);
                    
                    if (roomId != "") {
                        GameObject roomRendered = RenderRoomAtCoordinates(cellPosFromOrigin, roomId, dungeonParent);
                        RenderContentsAtCoordinates(cellPosFromOrigin, x, y, roomRendered);
                    }
                }
            }
        }

        private static void RenderContentsAtCoordinates(Vector2 cellPosFromOrigin, int x, int y, GameObject parent) {
            Vector3 cellPosFromOrigin3D = new Vector3(cellPosFromOrigin.x, cellPosFromOrigin.y, 0f);
            List<ContentPointData> contentPoints = _contentsMap.contentPointGrid.GetCellActual(x, y);
            foreach (ContentPointData contentPoint in contentPoints) {
                if (contentPoint.type == ContentType.Collectible) {
                    InstantiateGizmoTextAtCoordinates(cellPosFromOrigin3D + contentPoint.coordinates.position, "C", parent);
                }
                if (contentPoint.type == ContentType.Mob) {
                    InstantiateGizmoTextAtCoordinates(cellPosFromOrigin3D + contentPoint.coordinates.position, "M", parent);
                }
            }
        }

        private static Vector2 DetermineCellPosition(Vector2Int roomActualIndex) {
            float spaceBetweenRooms = Consts.Get<int>("SizeOfRoom_PX");
            return new Vector2(_dungeonOrigin.x + (roomActualIndex.x * spaceBetweenRooms),
                _dungeonOrigin.y + (roomActualIndex.y * spaceBetweenRooms));
        }
        
        private static int FindStartX(FlexGrid2DString map) {
            for (int x = map.getXSize()-1; x > 0; x--) {
                bool isColumnEmpty = true;
                for (int y = 0; y < map.getYSize()-1; y++) {
                    if (map.GetCellActual(x, y) != "") {
                        return x;
                    }
                }
            }

            return -1;
        }
        
        private static GameObject InstantiateGizmoTextAtCoordinates(Vector3 coordinates, string text, GameObject parent) {
            GameObject gizmoSquare = new GameObject("GizmoText");
            MeshRenderer renderer = gizmoSquare.AddComponent<MeshRenderer>();

            // Create a new material using the "TextMeshPro/Mobile/Distance Field" shader
            Material material = new Material(Shader.Find("TextMeshPro/Mobile/Distance Field"));
            renderer.material = material;

            // Load the custom font and assign it to the material's texture property
            Font font = Resources.Load<Font>("Kaph_Font/OpenType (.otf)/Kaph-Regular");
            material.SetTexture("_MainTex", font.material.mainTexture);

            TextMesh textMesh = gizmoSquare.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.characterSize = 0.05f;
            textMesh.fontSize = 300;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = Color.black;
            textMesh.font = font;
            
            renderer.sortingLayerName = "Gizmos"; // Set to the desired sorting layer name
            renderer.sortingOrder = 2; 

            gizmoSquare.transform.position = new Vector3(coordinates.x, coordinates.y, -5f);
            gizmoSquare.transform.parent = parent.transform;

            return gizmoSquare;
        }
        
        private static GameObject RenderRoomAtCoordinates(Vector2 coordinates, string roomId, GameObject dungeonParent) {
            GameObject roomPrefab = Generator.DungeonGenerator.GetRoomPrefabFromID(roomId);
            if (roomPrefab == null) {
                return null;
            }
            Vector3 position3D = new Vector3(coordinates.x, coordinates.y, 0);
            GameObject roomInstance = GameObject.Instantiate(roomPrefab, position3D, Quaternion.identity);
            roomInstance.transform.parent = dungeonParent.transform;

            return roomInstance;
        }

        private static Vector2 CalcDungeonOrigin(DungeonData dungeonData) {
            Vector2 dungeonOrigin = new Vector2();
            
            if (dungeonData.coordinates.x == 1) {
                dungeonOrigin.x = 7f + Consts.Get<int>("SizeOfRoom_PX") / 2;
                dungeonOrigin.y = dungeonData.coordinates.y;
            }
            if (dungeonData.coordinates.x == -1) {
                dungeonOrigin.x = -2.5f - dungeonData.dungeonWidth * Consts.Get<int>("SizeOfRoom_PX");
                dungeonOrigin.y = dungeonData.coordinates.y;
            }
            return dungeonOrigin;
        }
    }
}
