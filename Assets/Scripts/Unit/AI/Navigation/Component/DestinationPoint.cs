using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace Unit.AI {

    public class DestinationPoint : NavPoint {

        [SerializeField] public List<PointNeighbour> closestPoints;
        [SerializeField] private float maxRange = 7f;

        private void Start() {
            FindAccessiblePoints();
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

                ValidateNeighboursOnThisSide(ref leftNeighbours);
                ValidateNeighboursOnThisSide(ref rightNeighbours);
                ValidateNeighboursOnThisSide(ref topNeighbours);
                ValidateNeighboursOnThisSide(ref bottomNeighbours);

            }

            List<PointNeighbour> combinedNeighbours = new List<PointNeighbour>();
            combinedNeighbours.AddRange(leftNeighbours);
            combinedNeighbours.AddRange(rightNeighbours);
            combinedNeighbours.AddRange(topNeighbours);
            combinedNeighbours.AddRange(bottomNeighbours);

            closestPoints = combinedNeighbours;
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
            float leftAngle = -135f;
            float rightAngle = 135f;
            float bottomAngle = -225f;
            float topAngle = -45f;

            if (angle > leftAngle && angle <= rightAngle) {
                return Side.Left;
            } else if ((angle > rightAngle && angle <= 180f) || (angle > -180f && angle <= leftAngle)) {
                return Side.Right;
            } else if (angle > bottomAngle && angle <= topAngle) {
                return Side.Bottom;
            } else {
                return Side.Top;
            }
        }

        private float GetAngleToPoint(Vector2 position) {
            Vector2 directionToTestedPoint = (position - location).normalized;
            return Mathf.Atan2(directionToTestedPoint.y, directionToTestedPoint.x) * Mathf.Rad2Deg;
        }

        private PointNeighbour ConvertToPointNeighbour(DestinationPoint point) {
            return new PointNeighbour {
                point = point,
                distance = Vector2.Distance(transform.position, point.location)
            };
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
