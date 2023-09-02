using System.Collections.Generic;
using System.Linq;
using Unit.Util;
using UnityEngine;

namespace Unit.AI.Navigation {
    public class Pathfinder : MonoBehaviour, IBrainComponent {

        private GameObject target;
        public BrainBase Brain { get; set; }

        private DestinationPoint currentDestinationPoint;
        private DestinationPoint finalDestinationPoint;

        public Vector2Int currentDirection;
        
        [SerializeField] public float unitRangeDistance = 2f;
        [SerializeField] private List<DestinationPoint> allDestinationPoints;

        [SerializeField] private int nextDestinationPoint;
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

            if (Util.IsTargetCloseEnough(target, gameObject.transform.position, unitRangeDistance) || NoDestinationPointsNear()) {
                FreeMove();
                return;
            }

            currentDestinationPoint = FindClosestDestinationPoint(gameObject.transform.position);
            finalDestinationPoint = FindClosestDestinationPoint(target.transform.position);

            // If new destination point found we need to recalculate the path.
            if (pathToDestination.Count == 0 || finalDestinationPoint.location != pathToDestination[^1].location) {
                pathToDestination = FindShortestPath();
                nextDestinationPoint = 1;
            }

            // If we got inside (too close) into the point, that is not part of current path, then we restart the path
            float distanceToCurrentPoint = Vector2.Distance(transform.position, currentDestinationPoint.location);
            if (distanceToCurrentPoint <= currentDestinationPoint.minRange && 
                !Util.PathContainsDestinationPoint(pathToDestination, currentDestinationPoint)) {
                pathToDestination = FindShortestPath();
                nextDestinationPoint = 1;
            }
            
            nextDestinationPoint = FindNexDestinationPoint();
            
            GetDirection();
        }
        
        public void SendSignalToBrain(string param) {
            switch (param) {
                case "Jump":
                    Brain.ReceiveSignal(
                        new BrainSignal(
                            BrainSignalType.Navigation,
                            UnitAction.Jump
                            )   
                        );
                    break;
                case "Move Left":
                    Brain.ReceiveSignal(
                        new BrainSignal(
                            BrainSignalType.Navigation,
                            UnitAction.MoveLeft
                        )   
                    );
                    break;
                case "Move Right":
                    Brain.ReceiveSignal(
                        new BrainSignal(
                            BrainSignalType.Navigation,
                            UnitAction.MoveRight
                        )   
                    );
                    break;
            }
        }
        
        public void SelectTarget(GameObject target) {
            this.target = target;
        }
        
        public void ActionPointDetected() {
            if (currentDirection.y == 1) {
                SendSignalToBrain("Jump");
            }
        }

        private bool NoDestinationPointsNear() {
            DestinationPoint point = FindClosestDestinationPoint(gameObject.transform.position);
            return Vector2.Distance(point.location, gameObject.transform.position) > 20f;
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
                    float thiPathLength = Util.CalculatePathLength(thisPath);
                    if (thiPathLength > shortestPathLength) {
                        continue;
                    }
                
                    DestinationPoint pathHead = Util.GetPathHead(thisPath);
                    if (pathHead.location == targetPoint.location) {
                        if (Util.CalculatePathLength(thisPath) < shortestPathLength) {
                            shortestPathLength = Util.CalculatePathLength(thisPath);
                            continue;
                        }
                    };
                    
                    List<PointNeighbour> availablePoints = pathHead.closestPoints;

                    foreach (PointNeighbour availablePoint in availablePoints) {
                        List<DestinationPoint> newPath = Util.ClonePath(thisPath);
                        
                        if (Util.PathContainsDestinationPoint(newPath, availablePoint.point)) continue;
                        newPath.Add(availablePoint.point);
                    
                        if (Util.PathAlreadyCreated(possiblePaths, newPath)) continue;
                        
                        possiblePaths.Add(newPath);
                        actionPerfomed = true;
                    }
                }
            }

            return GetBestPath(possiblePaths, shortestPathLength);
        }

        private void GetDirection() {
            if (pathToDestination[nextDestinationPoint] == null) {
                currentDirection = new Vector2Int(0, 0);
            }
            
            if (transform.position.x > pathToDestination[nextDestinationPoint].location.x) {
                currentDirection.x = -1;
                SendSignalToBrain("Move Left");
            }
            else {
                currentDirection.x = 1;
                SendSignalToBrain("Move Right");
            }
            
            if (transform.position.y > pathToDestination[nextDestinationPoint].location.y) {
                currentDirection.y = -1;
            }
            else {
                currentDirection.y = 1;
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

        private int FindNexDestinationPoint() {
            float distanceToCurrentNextPoint = Vector2.Distance(transform.position, pathToDestination[nextDestinationPoint].location);
            
            if (distanceToCurrentNextPoint <= pathToDestination[nextDestinationPoint].minRange) {
                return nextDestinationPoint + 1;
            }

            return nextDestinationPoint;
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
                SendSignalToBrain("Move Right");
            }
            else {
                currentDirection.x = -1;
                SendSignalToBrain("Move Left");
            }
        }

        private List<DestinationPoint> GetAllDestinationPoints() =>
            FindObjectsOfType<DestinationPoint>()
                .Select(obj => obj.GetComponent<DestinationPoint>())
                .ToList();

        private bool CanAccessTargetPoint(DestinationPoint fromPoint, DestinationPoint targetPoint) {
            foreach (PointNeighbour pointInRange in fromPoint.closestPoints) {
                if (pointInRange.point.location == targetPoint.location) {
                    return true;
                }
            }

            return false;
        }

        private List<DestinationPoint> GetBestPath(List<List<DestinationPoint>> paths, float shortestDistance) {
            List<DestinationPoint> bestPath = null;
            foreach (List<DestinationPoint> path in paths) {
                if (Util.CalculatePathLength(path) > shortestDistance && 
                    Util.GetPathHead(path).location == finalDestinationPoint.location) {
                    continue;
                }

                bestPath = path;
            }

            return bestPath;
        }
    }
}