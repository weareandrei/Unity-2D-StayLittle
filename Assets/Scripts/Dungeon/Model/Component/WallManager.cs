using System.Collections.Generic;
using System.Linq;
using Dungeon.Model;
using HoneyGrid2D;
using UnityEngine;

namespace Dungeon.Model {
    public class WallManager : MonoBehaviour
    {
        private GameObject[] walls;
        public FlexGrid2DSpecial<GameObject> wallGrid; 
    
        private List<GameObject> topWalls = new List<GameObject>();
        private List<GameObject> bottomWalls = new List<GameObject>();
        private List<GameObject> leftWalls = new List<GameObject>();
        private List<GameObject> rightWalls = new List<GameObject>();

        private void Awake() {
            wallGrid = new FlexGrid2DSpecial<GameObject>(Dungeon.Consts.Get<int>("RoomSize") + 2,
                Dungeon.Consts.Get<int>("RoomSize") + 2);
        
            GetWalls();
            AssignWallsToGrid();
        }

        private void AssignWallsToGrid() {
            GetWallCoordinates();

            int topY, bottomY, leftX, rightX;
            CalculateEdgeCoordinates(out topY, out bottomY, out leftX, out rightX);

            GroupWallsBySide(topY, bottomY, leftX, rightX);

            SortWallsInGroups();
            SetWallsInGrid();
        }

        private void GetWalls() {
            walls = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.GetComponent<Wall>() == null)
                {
                    // If the child doesn't have a Wall component, add it
                    child.AddComponent<Wall>();
                }
                walls[i] = child;
            }
        }

        private void GetWallCoordinates() {
            foreach (var wallObject in walls) {
                Vector2Int coordinates = new Vector2Int(
                    Mathf.RoundToInt(wallObject.transform.localPosition.x),
                    Mathf.RoundToInt(wallObject.transform.localPosition.y));
                wallObject.GetComponent<Wall>().coordinates = coordinates;
            }
        }

        private void CalculateEdgeCoordinates(out int topY, out int bottomY, out int leftX, out int rightX) {
            topY = int.MinValue;
            bottomY = int.MaxValue;
            leftX = int.MaxValue;
            rightX = int.MinValue;

            foreach (GameObject wall in walls) {
                Vector2Int coordinates = wall.GetComponent<Wall>().coordinates;
            
                if (coordinates.y > topY)
                    topY = coordinates.y;
                if (coordinates.y < bottomY)
                    bottomY = coordinates.y;
                if (coordinates.x < leftX)
                    leftX = coordinates.x;
                if (coordinates.x > rightX)
                    rightX = coordinates.x;
            }
        }

        private void GroupWallsBySide(int topY, int bottomY, int leftX, int rightX) {
            foreach (GameObject wall in walls) {
                Vector2Int coordinates = wall.GetComponent<Wall>().coordinates;
            
                int roundedY = Mathf.RoundToInt(coordinates.y);
                int roundedX = Mathf.RoundToInt(coordinates.x);

                if (roundedY == topY) {
                    topWalls.Add(wall);
                } else if (roundedY == bottomY) {
                    bottomWalls.Add(wall);
                } else if (roundedX == leftX) {
                    leftWalls.Add(wall);
                } else if (roundedX == rightX) {
                    rightWalls.Add(wall);
                }
            }
        }
    
        private void SortWallsInGroups() {
            topWalls = topWalls.OrderBy(wall => wall.GetComponent<Wall>().coordinates.x).ToList();
            bottomWalls = bottomWalls.OrderBy(wall => wall.GetComponent<Wall>().coordinates.x).ToList();
            leftWalls = leftWalls.OrderByDescending(wall => wall.GetComponent<Wall>().coordinates.y).ToList();
            rightWalls = rightWalls.OrderByDescending(wall => wall.GetComponent<Wall>().coordinates.y).ToList();

        }
    
        private void SetWallsInGrid() {
            // TOP
            for (int x = 0; x < topWalls.Count; x++) {
                wallGrid.UpdateCell(x+1, 0, topWalls[x]);
            }
            
            // BOTTOM
            for (int x = 0; x < bottomWalls.Count; x++) {
                wallGrid.UpdateCell(x+1, Dungeon.Consts.Get<int>("RoomSize") + 1, bottomWalls[x]);
            }
            
            // LEFT
            for (int y = 0; y < leftWalls.Count; y++) {
                wallGrid.UpdateCell(0, y+1, leftWalls[y]);
            }
            
            // RIGHT
            for (int y = 0; y < rightWalls.Count; y++) {
                wallGrid.UpdateCell(Dungeon.Consts.Get<int>("RoomSize") + 1, y+1, rightWalls[y]);
            }
        }

        public void EnableWall(int wallX, int wallY) {
            GameObject thisWall = wallGrid.GetCellActual(wallX, wallY);
            if (thisWall == null) {
                // Debug.LogWarning("Wall GameObject not found at coordinates (" + wallX + ", " + wallY + ").");
                return;
            }

            Wall wallComponent = thisWall.GetComponent<Wall>();
            if (wallComponent == null) {
                // Debug.LogWarning("Wall component not found on the wall GameObject at coordinates (" + wallX + ", " + wallY + ").");
                return;
            }

            // Disable and deactivate the wall GameObject
            wallComponent.wallEnabled = true;
            thisWall.SetActive(true);

            // Update the wallGrid with the modified GameObject
            wallGrid.UpdateCell(wallX, wallY, thisWall);
        }
        
        public void DisableWall(int wallX, int wallY) {
            GameObject thisWall = wallGrid.GetCellActual(wallX, wallY);
            if (thisWall == null) {
                // Debug.LogWarning("Wall GameObject not found at coordinates (" + wallX + ", " + wallY + ").");
                return;
            }

            Wall wallComponent = thisWall.GetComponent<Wall>();
            if (wallComponent == null) {
                // Debug.LogWarning("Wall component not found on the wall GameObject at coordinates (" + wallX + ", " + wallY + ").");
                return;
            }

            // Disable and deactivate the wall GameObject
            wallComponent.wallEnabled = false;
            thisWall.SetActive(false);
        }
    }
}
