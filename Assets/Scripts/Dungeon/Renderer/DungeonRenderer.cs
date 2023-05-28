using Dungeon.Data;
using Dungeon.Model;
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
                    Vector2Int cellPosFromOrigin = new Vector2Int(_dungeonOrigin.x + ((-1)*x*_spaceBetweenDungeons), _dungeonOrigin.y + (y*_spaceBetweenDungeons));
                    string roomId = roomMap.map.GetCell(x, y);
                    if (roomId != "") {
                        RenderRoomAtCoordinates(cellPosFromOrigin, roomMap.map.GetCell(x,y), x,y);
                    }
                }
            }
        }
        
        private static GameObject InstantiateGizmoSquareAtCoordinates(Vector2Int coordinates, int x, int y, string roomId) {
            GameObject gizmoSquare = new GameObject("GizmoSquare");
            MeshRenderer renderer = gizmoSquare.AddComponent<MeshRenderer>();

            // Create a new material using the "TextMeshPro/Mobile/Distance Field" shader
            Material material = new Material(Shader.Find("TextMeshPro/Mobile/Distance Field"));
            renderer.material = material;

            // Load the custom font and assign it to the material's texture property
            Font font = Resources.Load<Font>("Kaph_Font/OpenType (.otf)/Kaph-Regular");
            material.SetTexture("_MainTex", font.material.mainTexture);

            TextMesh textMesh = gizmoSquare.AddComponent<TextMesh>();
            textMesh.text = "(" + x + "," + y + ")" + "\n" + roomId;
            textMesh.characterSize = 0.05f;
            textMesh.fontSize = 500;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = Color.black;
            textMesh.font = font;

            gizmoSquare.transform.position = new Vector3(coordinates.x, coordinates.y, -0.1f);
            return gizmoSquare;
        }
        
        private static void RenderRoomAtCoordinates(Vector2Int coordinates, string roomId, int x, int y) {
            GameObject roomPrefab = Generator.DungeonGenerator.GetRoomPrefabFromID(roomId);
            if (roomPrefab == null) {
                return;
            }
            Vector3 position3D = new Vector3(coordinates.x, coordinates.y, 0);
            GameObject.Instantiate(roomPrefab, position3D, Quaternion.identity);

            // Set the position of the rendered prefab
            // renderedPrefab.transform.position = coordinates;
            
            // Instantiate gizmo square with number inside
            InstantiateGizmoSquareAtCoordinates(coordinates, x, y, roomId);
        }

        private static Vector2Int CalcDungeonOrigin(Vector2Int coordinates) {
            // Should calculate based on Right or Left side
            return coordinates;
        }
    }
}
