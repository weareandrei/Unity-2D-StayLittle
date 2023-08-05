using System;
using UnityEngine;

namespace Unit.AI {
    
    public class Pathfinder {
        private DestinationPoint currentDestinationPoint;
        private DestinationPoint finalDestinationPoint;

        public void MoveToTarget(GameObject targetObject) {
            FindClosestDestinationPoint(targetObject);
        }
        
        public void FindDestinationToPoint(DestinationPoint finalDestinationPoint) {
            throw new System.NotImplementedException();
        }
        
        public void UseActionPoint() {
            throw new System.NotImplementedException();
        }

        public DestinationPoint FindClosestDestinationPoint(GameObject target) {
            throw new NotImplementedException();
        }

    }

    public enum ActorMovementActions {
        MoveLeft = 1,
        MoveRight = 2,
        Jump
    }
}