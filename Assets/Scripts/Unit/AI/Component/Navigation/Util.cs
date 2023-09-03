using System.Collections.Generic;
using UnityEngine;

namespace Unit.AI.Navigation {
    
    public class Util {
        
        public static bool IsTargetCloseEnough(GameObject target, Vector2 currentPosition, float minDistance) {
            Vector3 playerPosition = target.transform.position;

            float xDistance = Mathf.Abs(playerPosition.x - currentPosition.x);
            float yDistance = Mathf.Abs(playerPosition.y - currentPosition.y);

            return xDistance < minDistance && yDistance <= 1f;
        }

        public static float CalculatePathLength(List<DestinationPoint> path) {
            float pathLength = 0;
            
            for (int i = 1; i < path.Count-1; i++) {
                pathLength += Vector2.Distance(path[i - 1].location, path[i].location);
            }

            return pathLength;
        }

        public static List<DestinationPoint> ClonePath(List<DestinationPoint> path) {
            List<DestinationPoint> clonedList = new List<DestinationPoint>();
            foreach (DestinationPoint destinationPoint in path) {
                clonedList.Add(destinationPoint);
            }

            return clonedList;
        }
        
        public static bool PathsAreIdentical(List<DestinationPoint> path1, List<DestinationPoint> path2) {
            if (path1.Count != path2.Count)
                return false;

            for (int i = 0; i < path1.Count; i++) {
                if (path1[i].location != path2[i].location)
                    return false;
            }

            return true;
        }
        
        public static DestinationPoint GetPathHead(List<DestinationPoint> path) {
            int pathLength = path.Count;
            return path[pathLength - 1];
        }
        
        public static bool PathContainsDestinationPoint(List<DestinationPoint> path, DestinationPoint point) {
            foreach (DestinationPoint destinationPoint in path) {
                if (destinationPoint.location == point.location) {
                    return true;
                }
            }

            return false;
        }
        
        public static bool PathAlreadyCreated(List<List<DestinationPoint>> allPaths, List<DestinationPoint> thisPath) {
            foreach (List<DestinationPoint> path in allPaths) {
                if (Util.PathsAreIdentical(path, thisPath)) {
                    return true;
                }
            }

            return false;
        }
        
    }
    
}