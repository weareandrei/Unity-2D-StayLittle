using System;
using UnityEngine;

namespace Unit.AI {
    
    public class Pathfinder : MonoBehaviour {
        private DestinationPoint currentDestinationPoint;
        private DestinationPoint finalDestinationPoint;

        private GameObject target;
        public Vector2Int currentDirection = new Vector2Int();

        private void FixedUpdate() {
            if (!target) {
                currentDirection = new Vector2Int(0, 0);
            }
            SelectPathToTarget(target);
            GetDirection();
        }

        public void SelectTarget(GameObject target) {
            this.target = target;
        }
        
        private void SelectPathToTarget(GameObject targetObject) {
            DestinationPoint targetClosestPoint = FindClosestDestinationPoint(targetObject);
            currentDestinationPoint = FinaNextDestinationPointToTarget(targetClosestPoint);
        }

        private void GetDirection() {
            if (currentDestinationPoint.location.y > target.transform.position.y) {
                currentDirection.y = 1;
            }
            else {
                currentDirection.y = -1;
            }
            
            if (currentDestinationPoint.location.x > target.transform.position.x) {
                currentDirection.x = 1;
            }
            else {
                currentDirection.x = -1;
            }
            
        }
        
        private DestinationPoint FindClosestDestinationPoint(GameObject target) {
            throw new NotImplementedException();
        }

        private DestinationPoint FinaNextDestinationPointToTarget(DestinationPoint target) {
            throw new NotImplementedException();
        }
        
        public void FindDestinationToPoint(DestinationPoint finalDestinationPoint) {
            throw new System.NotImplementedException();
        }
        
        private void UseActionPoint() {
            throw new System.NotImplementedException();
        }
        
    }

    public enum ActorMovementActions {
        MoveLeft = 1,
        MoveRight = 2,
        Jump
    }
}