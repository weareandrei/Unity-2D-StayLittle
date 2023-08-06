using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.AI {
    
    public class DestinationPoint : NavPoint {
        
        [SerializeField] private List<PointInRange> closestPoints;
        [SerializeField] private float minRange = 35f;

        private void Start() {
            DestinationPoint[] allDestinationPoints = FindObjectsOfType<DestinationPoint>();

            foreach (DestinationPoint otherPoint in allDestinationPoints) {
                if (otherPoint != this) {
                    float distance = Vector2.Distance(transform.position, otherPoint.transform.position);
                    
                    if (distance < minRange) {
                        PointInRange newPointInRange = 
                            new PointInRange {
                                distance = distance, 
                                point = otherPoint
                            };

                        if (PointIsReachable(otherPoint)) {
                            closestPoints.Add(newPointInRange);
                        }
                        
                    }
                }
            }

        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            try {
            } catch { /*ignored*/ }
        }

        private bool PointIsReachable(DestinationPoint destinationPoint) {
            Vector2 origin = location;
            Vector2 target = destinationPoint.location;

            RaycastHit2D hit = Physics2D.Raycast(
                origin, target - origin, 
                Vector2.Distance(origin, target), 
                LayerMask.GetMask("NavigationCollider"));

            if (hit.collider != null) {
                return false;
            }

            return true;
        }

        
    }

    [Serializable]
    public struct PointInRange {
        public float distance;
        public DestinationPoint point;
    }
    
}