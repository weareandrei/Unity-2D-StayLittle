using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unit.AI {
    
    public class Pathfinder : MonoBehaviour {
        private DestinationPoint currentDestinationPoint;
        private DestinationPoint finalDestinationPoint;

        private GameObject target;
        public Vector2Int currentDirection;

        [SerializeField] private float minDistanceToFreeMove = 3f;
        [SerializeField] private List<DestinationPoint> allDestinationPoints;

        [SerializeField] private List<DestinationPoint> pathToDestination;
        
        private void Start() {
            currentDirection = new Vector2Int(0, 0);
            allDestinationPoints = GetAllDestinationPoints();
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
            
            pathToDestination = FindShortestPath(gameObject.transform.position, target.transform.position);
            GetDirection();
        }

        private bool IsPlayerCloseEnough() {
            return Vector2.Distance(target.transform.position, gameObject.transform.position)
                   < minDistanceToFreeMove;
        }
        private bool NoDestinationPointsNear() {
            DestinationPoint point = FindClosestDestinationPoint(gameObject.transform.position);
            return Vector2.Distance(point.location, gameObject.transform.position)
                   < minDistanceToFreeMove;
        }

        public void SelectTarget(GameObject target) {
            this.target = target;
        }

        private List<DestinationPoint> FindShortestPath(Vector2 fromPosition, Vector2 toPosition) {
            DestinationPoint fromPoint = FindClosestDestinationPoint(fromPosition);
            DestinationPoint targetPoint = FindClosestDestinationPoint(toPosition);
            
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
                    List<PointNeighbour> availablePoints = pathHead.closestPoints;

                    if (CanAccessTargetPoint(pathHead, targetPoint)) {
                        thisPath.Add(targetPoint);
                        if (CalculatePathLength(thisPath) < shortestPathLength) {
                            shortestPathLength = CalculatePathLength(thisPath);
                        }

                        actionPerfomed = true;
                    }
                
                    foreach (PointNeighbour availablePoint in availablePoints) {
                        List<DestinationPoint> newPath = thisPath;
                        newPath.Add(availablePoint.point);
                    
                        if (!PathAlreadyCreated(possiblePaths, newPath)) {
                            possiblePaths.Add(newPath);
                        }
                        
                        actionPerfomed = true;
                    }
                }
            }

            return GetBestPath(possiblePaths, shortestPathLength);
        }

        private void GetDirection() {
            if (pathToDestination == null || currentDestinationPoint == null) {
                currentDirection = new Vector2Int(0, 0);
            }
            if (currentDestinationPoint.location.x > target.transform.position.x) {
                currentDirection.x = -1;
            }
            else {
                currentDirection.x = 1;
            }
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


        public void FindDestinationToPoint(DestinationPoint finalDestinationPoint) {
            throw new System.NotImplementedException();
        }
        
        private void UseActionPoint() {
            throw new System.NotImplementedException();
        }

        private void FreeMove() {
            if (gameObject.transform.position.y < target.transform.position.y) {
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
                if (CalculatePathLength(path) > shortestDistance) {
                    continue;
                }

                bestPath = path;
            }

            return bestPath;
        }

    }

    public enum ActorMovementActions {
        MoveLeft = 1,
        MoveRight = 2,
        Jump
    }

}