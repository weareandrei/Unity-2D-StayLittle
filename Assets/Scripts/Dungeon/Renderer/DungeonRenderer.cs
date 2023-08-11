using System.Collections;
using Content.Util;
using System.Collections.Generic;
using Content;
using Dungeon.Data;
using Dungeon.Model;
using HoneyGrid2D;
using Interaction;
using UnityEngine;
    
namespace Dungeon.Renderer {
    public class DungeonRenderer {

        private int _spaceBetweenDungeons = 12;
        
        private RoomMap _roomMap;
        private ContentsMap _contentsMap;
        
        // Based on this we can know if this is a Right or Left side Dungeon
        private Vector2 _dungeonOrigin;

        // Here we Render only 1 specific Dungeon
        public void RenderDungeon(DungeonData dungeonData, DungeonMapData dungeonMapData) {
            _roomMap = dungeonMapData.roomMap;
            _contentsMap = dungeonMapData.contentsMap;

            _dungeonOrigin = CalcDungeonOrigin(dungeonData);

            GameObject dungeonParent = new GameObject("Dungeon");
            DungeonDataContainer newDataContainer = dungeonParent.AddComponent<DungeonDataContainer>();
            newDataContainer.data = dungeonData;

            DungeonRendererHelper helper = dungeonParent.AddComponent<DungeonRendererHelper>();
            helper.SetDungeonRenderer(this);
            helper.StartRenderCoroutine(dungeonParent);
        }

        private void RenderRooms(GameObject dungeonParent) {
            _roomMap.map.LoopThroughCells((x, y) => {
                Vector2 cellPosFromOrigin = DetermineCellPosition(new Vector2Int(x, y));
                string roomId = _roomMap.map.GetCellActual(_roomMap.map.getXSize() - 1 - x, y);

                if (roomId != "") {
                    GameObject roomRendered = RenderRoomAtCoordinates(cellPosFromOrigin, roomId, dungeonParent);
                    dungeonParent.GetComponent<DungeonRendererHelper>().renderedRooms.Add(roomRendered);
                }
                return LoopState.Continue;
            });
        }

        public void RenderContents(GameObject dungeonParent) {
            int counter = 0;
            _roomMap.map.LoopThroughCells((x, y) => {
                Vector2 cellPosFromOrigin = DetermineCellPosition(new Vector2Int(x, y));
                string roomId = _roomMap.map.GetCellActual(_roomMap.map.getXSize() - 1 - x, y);

                if (roomId != "") {
                    GameObject roomRendered = dungeonParent.GetComponent<DungeonRendererHelper>().renderedRooms[counter];
                    RenderThisRoomContents(_roomMap.map.getXSize() - 1 - x, y, roomRendered);
                    counter++;
                }
                return LoopState.Continue;
            });
        }

        private void RenderThisRoomContents(int x, int y, GameObject roomRendered) {
            List<GameObject> contentPoints = roomRendered.GetComponent<Room>().GetContentPoints();
            List<ContentPayload> contentPayloads = _contentsMap.map.GetCellActual(x, y).payloads;
            for (int i = 0; i < contentPoints.Count; i++) {
                if (i < contentPayloads.Count) {
                    // contentPoints[i].GetComponent<ContentPoint>().payload = contentPayloads[i];
                    RenderContentPoint(contentPoints[i], roomRendered);
                } else {
                    // Handle the case where there are fewer ContentPayloads than ContentPoints
                    // For example, you could assign a default or null payload.
                    //   to something here
                }
            }

            EnableWalls(x, y, roomRendered);
        }

        private void RenderContentPoint(GameObject contentPoint, GameObject roomRendered) {
            ContentPayload payload = contentPoint.GetComponent<ContentPoint>().payload;
            Transform coordinates = contentPoint.transform;
                
            switch (payload.type) {
                case ContentType.Mob:
                    GameObject mob = RenderMob(coordinates, payload.ContentID);
                    mob.transform.parent = roomRendered.transform;
                    break;
                case ContentType.Collectible:
                    GameObject collectible = RenderCollectible(coordinates, payload.ContentID);
                    collectible.AddComponent<Collectible>().payload = payload;
                    collectible.transform.parent = roomRendered.transform;;
                    break;
            }

        }

        private GameObject RenderCollectible(Transform coordinates, string id) {
            GameObject collectiblePrefab = GetContent.GetPrefabByID(id);
            GameObject collectible = GameObject.Instantiate(collectiblePrefab, coordinates.position, Quaternion.identity);

            return collectible;
        }

        private GameObject RenderMob(Transform coordinates, string id) {
            GameObject collectiblePrefab = GetContent.GetPrefabByID(id);
            GameObject collectible = GameObject.Instantiate(collectiblePrefab, coordinates.position, Quaternion.identity);

            return collectible;
        }

        private void EnableWalls(int room_x, int room_y, GameObject roomRendered) {
            FlexGrid2DBool wallsMap = _contentsMap.map.GetCellActual(room_x, room_y).walls;
            
            wallsMap.LoopThroughCells((wall_x, wall_y) => {
                if (wallsMap.GetCellActual(wall_x, wall_y) == true) {
                    roomRendered.transform.Find("Walls").GetComponent<WallManager>().EnableWall(wall_x, wall_y);
                } else {
                    roomRendered.transform.Find("Walls").GetComponent<WallManager>().DisableWall(wall_x, wall_y);
                }
                return LoopState.Continue;
            });
            
        }

        private Vector2 DetermineCellPosition(Vector2Int roomActualIndex) {
            float spaceBetweenRooms = Consts.Get<float>("SizeOfRoom_PX");
            return new Vector2(_dungeonOrigin.x + (roomActualIndex.x * spaceBetweenRooms),
                _dungeonOrigin.y + (roomActualIndex.y * spaceBetweenRooms));
        }
        
        private int FindStartX(FlexGrid2DString map) {
            int returnX = -1;
            map.LoopThroughCells((x, y) => {
                if (map.GetCellActual(x, y) != "") {
                    returnX = x;
                    return LoopState.Break;
                }

                return LoopState.Continue;
            });

            return returnX;
        }
        
        // private GameObject InstantiateGizmoTextAtCoordinates(Vector3 coordinates, string text, GameObject parent) {
        //     GameObject gizmoSquare = new GameObject("GizmoText");
        //     MeshRenderer renderer = gizmoSquare.AddComponent<MeshRenderer>();
        //
        //     // Create a new material using the "TextMeshPro/Mobile/Distance Field" shader
        //     Material material = new Material(Shader.Find("TextMeshPro/Mobile/Distance Field"));
        //     renderer.material = material;
        //
        //     // Load the custom font and assign it to the material's texture property
        //     Font font = Resources.Load<Font>("Kaph_Font/OpenType (.otf)/Kaph-Regular");
        //     material.SetTexture("_MainTex", font.material.mainTexture);
        //
        //     TextMesh textMesh = gizmoSquare.AddComponent<TextMesh>();
        //     textMesh.text = text;
        //     textMesh.characterSize = 0.05f;
        //     textMesh.fontSize = 300;
        //     textMesh.anchor = TextAnchor.MiddleCenter;
        //     textMesh.color = Color.black;
        //     textMesh.font = font;
        //     
        //     renderer.sortingLayerName = "Gizmos"; // Set to the desired sorting layer name
        //     renderer.sortingOrder = 2; 
        //
        //     gizmoSquare.transform.position = new Vector3(coordinates.x, coordinates.y, -5f);
        //     gizmoSquare.transform.parent = parent.transform;
        //
        //     return gizmoSquare;
        // }
        
        private GameObject RenderRoomAtCoordinates(Vector2 coordinates, string roomId, GameObject dungeonParent) {
            GameObject roomPrefab = Generator.DungeonGenerator.GetRoomPrefabFromID(roomId);
            if (roomPrefab == null) {
                return null;
            }
            Vector3 position3D = new Vector3(coordinates.x, coordinates.y, 0);
            GameObject roomInstance = GameObject.Instantiate(roomPrefab, position3D, Quaternion.identity);
            roomInstance.transform.parent = dungeonParent.transform;

            return roomInstance;
        }

        private Vector2 CalcDungeonOrigin(DungeonData dungeonData) {
            Vector2 dungeonOrigin = new Vector2();
            
            if (dungeonData.coordinates.x == 1) {
                dungeonOrigin.x = 7f + Consts.Get<float>("SizeOfRoom_PX") / 2;
                dungeonOrigin.y = dungeonData.coordinates.y;
            }
            if (dungeonData.coordinates.x == -1) {
                dungeonOrigin.x = -2.5f - dungeonData.dungeonWidth * Consts.Get<float>("SizeOfRoom_PX");
                dungeonOrigin.y = dungeonData.coordinates.y;
            }
            return dungeonOrigin;
        }
        
        
        
        private class DungeonRendererHelper : MonoBehaviour {
            
            public List<GameObject> renderedRooms = new List<GameObject>();
            private DungeonRenderer dungeonRenderer; // Reference to DungeonRenderer instance
    
            public void SetDungeonRenderer(DungeonRenderer renderer) {
                dungeonRenderer = renderer;
            }

            public void StartRenderCoroutine(GameObject dungeonParent) {
                StartCoroutine(RenderCoroutine(dungeonParent));
            }

            private IEnumerator RenderCoroutine(GameObject dungeonParent) {
                dungeonRenderer.RenderRooms(dungeonParent);
                yield return new WaitForEndOfFrame();
                dungeonRenderer.RenderContents(dungeonParent);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
