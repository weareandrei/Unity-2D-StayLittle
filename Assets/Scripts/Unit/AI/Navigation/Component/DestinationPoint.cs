using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.AI {

    public class DestinationPoint : NavPoint {
        
        [SerializeField] public List<PointNeighbour> closestPoints;
        [SerializeField] private float maxRange = 7f;

        private void Start() {
            FindAccessiblePoints();
        }

        public DestinationPoint(List<PointNeighbour> closestPoints, float maxRange, Vector2 location) {
            this.closestPoints = closestPoints;
            this.maxRange = maxRange;
            this.location = location;
        }

        private void FindAccessiblePoints() {
            DestinationPoint[] allDestinationPoints = FindObjectsOfType<DestinationPoint>();

            foreach (DestinationPoint point in allDestinationPoints) {
                if (point == this) continue;
                if (point != this && PointAccessible(point)) {
                    closestPoints.Add(ConvertToPointNeighbour(point));
                }
            }
            
            FilterLegitNeighbours();
        }
        
        private bool PointAccessible(DestinationPoint point) {
            float distance = Vector2.Distance(transform.position, point.location);

            if (distance > maxRange) return false;
            
            if (!PointPhysicallyReachable(point)) return false;

            return true;
        }

        private bool PointPhysicallyReachable(DestinationPoint point) {
            Vector2 origin = location;
            Vector2 target = point.location;

            RaycastHit2D hit = Physics2D.Raycast(
                origin, target - origin,
                Vector2.Distance(origin, target),
                LayerMask.GetMask("Navigation"));

            if (hit.collider != null && hit.collider.gameObject.tag == "NavCollider") {
                return false;
            }

            return true;
        }

        private void FilterLegitNeighbours() {
            List<PointNeighbour> leftNeighbours = new List<PointNeighbour>();
            List<PointNeighbour> rightNeighbours = new List<PointNeighbour>();
            List<PointNeighbour> topNeighbours = new List<PointNeighbour>();
            List<PointNeighbour> bottomNeighbours = new List<PointNeighbour>();

            foreach (PointNeighbour neighbourPoint in closestPoints) {
                AssignPointSides(ref leftNeighbours, ref rightNeighbours, ref topNeighbours, ref bottomNeighbours, neighbourPoint);
            }
            
            ValidateNeighboursOnThisSide(ref leftNeighbours);
            ValidateNeighboursOnThisSide(ref rightNeighbours);
            ValidateNeighboursOnThisSide(ref topNeighbours);
            ValidateNeighboursOnThisSide(ref bottomNeighbours);

            List<PointNeighbour> combinedNeighbours = new List<PointNeighbour>();
            combinedNeighbours.AddRange(leftNeighbours);
            combinedNeighbours.AddRange(rightNeighbours);
            combinedNeighbours.AddRange(topNeighbours);
            combinedNeighbours.AddRange(bottomNeighbours);

            closestPoints = combinedNeighbours;
        }

        private void AssignPointSides(ref List<PointNeighbour> leftNeighbours, ref List<PointNeighbour> rightNeighbours, ref List<PointNeighbour> topNeighbours, ref List<PointNeighbour> bottomNeighbours, PointNeighbour neighbourPoint) {
            Side pointSide = GetSideForPoint(neighbourPoint);
            switch (pointSide) {
                case Side.Left:
                    leftNeighbours.Add(neighbourPoint);
                    break;
                case Side.Right:
                    rightNeighbours.Add(neighbourPoint);
                    break;
                case Side.Top:
                    topNeighbours.Add(neighbourPoint);
                    break;
                case Side.Bottom:
                    bottomNeighbours.Add(neighbourPoint);
                    break;
            }
        }

        private void ValidateNeighboursOnThisSide(ref List<PointNeighbour> neighbours) {
            PointNeighbour closestNeighbour = FindClosestNeighbour(neighbours);
            float smallestDistance = closestNeighbour.distance;
            float distanceThreshold = smallestDistance * 0.15f;

            foreach (PointNeighbour neighbour in neighbours) {
                float diffInDistance = Mathf.Abs(neighbour.distance - smallestDistance);
                float diffInY = Mathf.Abs(neighbour.point.location.y - closestNeighbour.point.location.y);
                
                if (diffInDistance <= distanceThreshold && diffInY >= 1.5f) {
                    continue;
                }
                
                neighbours.Remove(neighbour);
            }
        }

        private PointNeighbour FindClosestNeighbour(List<PointNeighbour> neighbours) =>
            neighbours
                .OrderBy(neighbour => neighbour.distance)
                .First();
        
        private Side GetSideForPoint(PointNeighbour pointNeighbour) {
            float angle = GetAngleToPoint(pointNeighbour.point.location);
            
            // Define angle ranges for each side (in degrees)
            float leftAngle = 135f;
            float rightAngle = 315;
            float topAngle = 225;
            float bottomAngle = 45f;

            if (angle > leftAngle && angle <= leftAngle + 90f) {
                return Side.Left;
            } else if ((angle > rightAngle && angle <= rightAngle + 45f) || (angle >= 0 && angle <= 45)) {
                return Side.Right;
            } else if (angle > topAngle && angle <= topAngle + 90f) {
                return Side.Top;
            } else if (angle > bottomAngle && angle <= bottomAngle + 90f) {
                return Side.Bottom;
            }

            throw new Exception();
        }

        private float GetAngleToPoint(Vector2 position) {
            Vector2 directionToTestedPoint = (position - location).normalized;
            float angle = Mathf.Atan2(directionToTestedPoint.y, directionToTestedPoint.x) * Mathf.Rad2Deg;
    
            if (angle < 0) {
                angle += 360f;
            }
    
            return angle;
        }


        private PointNeighbour ConvertToPointNeighbour(DestinationPoint point) {
            return new PointNeighbour {
                point = point,
                distance = Vector2.Distance(transform.position, point.location)
            };
        }
        
        // public DestinationPoint Clone() {
        //     DestinationPoint clone = new DestinationPoint(closestPoints, maxRange, location);
        //     return clone;
        // }
        
         
        // OnDrawGizmosSelected
        
        private void OnDrawGizmos () {
            if (gizmoColor == default) {
                gizmoColor = Random.ColorHSV(0.5f, 0.5f, 0.2f, 0.2f, 1f, 1f, 0.5f, 0.5f); // 20% transparent
            }
            Handles.color = gizmoColor;
            Handles.DrawWireDisc(
                transform.position, Vector3.forward, maxRange);
            Handles.DrawWireDisc(
                transform.position, Vector3.forward, maxRange / 10);

        }

    }

    public enum Side {
        Left,
        Right,
        Bottom,
        Top
    }

    [Serializable]
    public struct PointNeighbour {
        public float distance;
        public DestinationPoint point;
    }
}
