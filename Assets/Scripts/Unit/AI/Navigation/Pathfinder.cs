using System;
using UnityEngine;

namespace Unit.AI {
    
    public class Pathfinder : MonoBehaviour {
        private DestinationPoint currentDestinationPoint;
        private DestinationPoint finalDestinationPoint;

        private GameObject target;
        public Vector2Int currentDirection;

        [SerializeField] private float minDistanceToFreeMove = 30f; 

        private void Start() {
            currentDirection = new Vector2Int(0, 0);
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
            SelectPathToTarget(target);
            GetDirection();
        }

        private bool IsPlayerCloseEnough() {
            return Vector2.Distance(target.transform.position, gameObject.transform.position)
                   < minDistanceToFreeMove;
        }
        private bool NoDestinationPointsNear() {
            DestinationPoint point = FindClosestDestinationPoint(gameObject);
            return Vector2.Distance(point.location, gameObject.transform.position)
                   < minDistanceToFreeMove;
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
        
    }

    public enum ActorMovementActions {
        MoveLeft = 1,
        MoveRight = 2,
        Jump
    }
}