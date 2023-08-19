using System;
using System.Collections.Generic;
using System.Linq;
using Unit.Controller;
using UnityEngine;

namespace Unit.AI {
    
    public class Pathfinder : MonoBehaviour {
        private DestinationPoint currentDestinationPoint;
        [SerializeField] private int nextDestinationPoint;
        private DestinationPoint finalDestinationPoint;

        private GameObject target;
        public Vector2Int currentDirection;
        public List<UnitMovementActions> actionsAwaiting;
        
        [SerializeField] private float minDistanceToFreeMove = 3f;
        [SerializeField] private List<DestinationPoint> allDestinationPoints;

        [SerializeField] private List<DestinationPoint> pathToDestination;
        
        
        private void Start() {
            currentDirection = new Vector2Int(0, 0);
            allDestinationPoints = GetAllDestinationPoints();
            actionsAwaiting = new List<UnitMovementActions>();
        }

        private void FixedUpdate() {
            if (!target) {
                currentDirection = new Vector2Int(0, 0);
                return;
            }

            if (IsPlayerCloseEnough() || NoDestinationPointsNear()) {
                FreeMove();
                return;
            }

            currentDestinationPoint = FindClosestDestinationPoint(gameObject.transform.position);
            finalDestinationPoint = FindClosestDestinationPoint(target.transform.position);

            // If new destination point found we need to recalculate the path.
            if (pathToDestination == null || finalDestinationPoint.location != pathToDestination[^1].location) {
                pathToDestination = FindShortestPath();
                nextDestinationPoint = 1;
            }
            
            // If we got inside (too close) into the point, that is not part of current path, then we restart the path
            float distanceToCurrentPoint = Vector2.Distance(transform.position, currentDestinationPoint.location);
            if (distanceToCurrentPoint <= currentDestinationPoint.minRange && 
                !PathContainsDestinationPoint(pathToDestination, currentDestinationPoint)) {
                pathToDestination = FindShortestPath();
                nextDestinationPoint = 1;
            }
            
            nextDestinationPoint = FindNexDestinationPoint();
            
            GetDirection();
        }
        
        public void ActionPointDetected() {
            if (currentDirection.y == 1) {
                actionsAwaiting.Add(UnitMovementActions.Jump);
            }
        }

        private bool IsPlayerCloseEnough() {
            Vector3 playerPosition = target.transform.position;
            Vector3 thisPosition = gameObject.transform.position;

            float xDistance = Mathf.Abs(playerPosition.x - thisPosition.x);
            float yDistance = Mathf.Abs(playerPosition.y - thisPosition.y);

            return xDistance < minDistanceToFreeMove && yDistance <= 1f;
        }

        
        private bool NoDestinationPointsNear() {
            DestinationPoint point = FindClosestDestinationPoint(gameObject.transform.position);
            return Vector2.Distance(point.location, gameObject.transform.position)
                   > 20f;
        }

        public void SelectTarget(GameObject target) {
            this.target = target;
        }

        private List<DestinationPoint> FindShortestPath() {
            DestinationPoint fromPoint = currentDestinationPoint;
            DestinationPoint targetPoint = finalDestinationPoint;
            
            List<List<DestinationPoint>> possiblePaths = new List<List<DestinationPoint>>();
            float shortestPathLength = float.MaxValue;

            // Initialize the path's list with the first path
            // , obviously, any path here will contain fromPoint 
            possiblePaths.Add(
                new List<DestinationPoint> {
                    fromPoint
                });

            bool actionPerfomed = true;
            while (actionPerfomed) {
                actionPerfomed = false;
                
                for (int i = 0; i < possiblePaths.Count; i++) {
                    List<DestinationPoint> thisPath = possiblePaths[i];
                    float thiPathLength = CalculatePathLength(thisPath);
                    if (thiPathLength > shortestPathLength) {
                        continue;
                    }
                
                    DestinationPoint pathHead = GetPathHead(thisPath);
                    if (pathHead.location == targetPoint.location) {
                        if (CalculatePathLength(thisPath) < shortestPathLength) {
                            shortestPathLength = CalculatePathLength(thisPath);
                            continue;
                        }
                    };
                    
                    List<PointNeighbour> availablePoints = pathHead.closestPoints;

                    foreach (PointNeighbour availablePoint in availablePoints) {
                        List<DestinationPoint> newPath = ClonePath(thisPath);
                        
                        if (PathContainsDestinationPoint(newPath, availablePoint.point)) continue;
                        newPath.Add(availablePoint.point);
                    
                        if (PathAlreadyCreated(possiblePaths, newPath)) continue;
                        
                        possiblePaths.Add(newPath);
                        actionPerfomed = true;
                    }
                }
            }

            return GetBestPath(possiblePaths, shortestPathLength);
        }

        private bool PathContainsDestinationPoint(List<DestinationPoint> path, DestinationPoint point) {
            foreach (DestinationPoint destinationPoint in path) {
                if (destinationPoint.location == point.location) {
                    return true;
                }
            }

            return false;
        }

        private void GetDirection() {
            if (pathToDestination[nextDestinationPoint] == null) {
                currentDirection = new Vector2Int(0, 0);
            }
            
            if (transform.position.x > pathToDestination[nextDestinationPoint].location.x) {
                currentDirection.x = -1;
            }
            else {
                currentDirection.x = 1;
            }
            
            if (transform.position.y > pathToDestination[nextDestinationPoint].location.y) {
                currentDirection.y = -1;
            }
            else {
                currentDirection.y = 1;
            }
        }

        private DestinationPoint GetNextDestinationPointToReach() {
            for (int i = 0; i < pathToDestination.Count; i++) {
                DestinationPoint point = pathToDestination[i];
                if (point.location == currentDestinationPoint.location) {
                    if (i+1 < pathToDestination.Count ) {
                        float distanceToCurrentPoint = Vector2.Distance(transform.position, pathToDestination[i].location);
                        if (distanceToCurrentPoint > pathToDestination[i].minRange) {
                            return pathToDestination[i];
                        }
                        return pathToDestination[i + 1];
                    }
                }
            }
            
            return null;
        }
        
        private DestinationPoint FindClosestDestinationPoint(Vector2 targetPosition) {
            float minDistance = float.MaxValue;
            DestinationPoint closestPoint = null;

            foreach (DestinationPoint destinationPoint in allDestinationPoints) {
                float distance = Vector2.Distance(targetPosition, destinationPoint.location);
                if (distance < minDistance) {
                    minDistance = distance;
                    closestPoint = destinationPoint;
                }
            }

            return closestPoint;
        }

        private int FindNexDestinationPoint() {
            float distanceToCurrentNextPoint = Vector2.Distance(transform.position, pathToDestination[nextDestinationPoint].location);
            
            if (distanceToCurrentNextPoint <= pathToDestination[nextDestinationPoint].minRange) {
                return nextDestinationPoint + 1;
            }

            return nextDestinationPoint;
        }

        private void UseActionPoint() {
            throw new System.NotImplementedException();
        }

        private void FreeMove() {
            if (Mathf.Abs(gameObject.transform.position.y - target.transform.position.y) >= 1f) {
                currentDirection.y = 1;
            }
            else {
                currentDirection.y = -1;
            }
            
            if (gameObject.transform.position.x < target.transform.position.x) {
                currentDirection.x = 1;
            }
            else {
                currentDirection.x = -1;
            }
        }

        private List<DestinationPoint> GetAllDestinationPoints() =>
            FindObjectsOfType<DestinationPoint>()
                .Select(obj => obj.GetComponent<DestinationPoint>())
                .ToList();

        private DestinationPoint GetPathHead(List<DestinationPoint> path) {
            int pathLength = path.Count;
            return path[pathLength - 1];
        }

        private bool PathAlreadyCreated(List<List<DestinationPoint>> allPaths, List<DestinationPoint> thisPath) {
            foreach (List<DestinationPoint> path in allPaths) {
                if (PathsAreIdentical(path, thisPath)) {
                    return true;
                }
            }

            return false;
        }

        public bool PathsAreIdentical(List<DestinationPoint> path1, List<DestinationPoint> path2) {
            if (path1.Count != path2.Count)
                return false;

            for (int i = 0; i < path1.Count; i++) {
                if (path1[i].location != path2[i].location)
                    return false;
            }

            return true;
        }

        private bool CanAccessTargetPoint(DestinationPoint fromPoint, DestinationPoint targetPoint) {
            foreach (PointNeighbour pointInRange in fromPoint.closestPoints) {
                if (pointInRange.point.location == targetPoint.location) {
                    return true;
                }
            }

            return false;
        }

        private float CalculatePathLength(List<DestinationPoint> path) {
            float pathLength = 0;
            
            for (int i = 1; i < path.Count-1; i++) {
                pathLength += Vector2.Distance(path[i - 1].location, path[i].location);
            }

            return pathLength;
        }

        private List<DestinationPoint> GetBestPath(List<List<DestinationPoint>> paths, float shortestDistance) {
            List<DestinationPoint> bestPath = null;
            foreach (List<DestinationPoint> path in paths) {
                if (CalculatePathLength(path) > shortestDistance && 
                    GetPathHead(path).location == finalDestinationPoint.location) {
                    continue;
                }

                bestPath = path;
            }

            return bestPath;
        }

        private List<DestinationPoint> ClonePath(List<DestinationPoint> path) {
            List<DestinationPoint> clonedList = new List<DestinationPoint>();
            foreach (DestinationPoint destinationPoint in path) {
                clonedList.Add(destinationPoint);
            }

            return clonedList;
        }

        public List<UnitMovementActions> GetAwaitingActions() {
            List<UnitMovementActions> actions = new List<UnitMovementActions>(actionsAwaiting);
            
            actionsAwaiting.Clear();
            return actions;
        }

    }

}