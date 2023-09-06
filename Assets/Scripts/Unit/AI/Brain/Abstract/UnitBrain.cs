using System;
using System.Collections.Generic;
using Unit.Base;
using Unit.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.AI {
    
    public abstract class UnitBrain : BrainBase {
        
        protected new NPCUnit thisUnit;
        
        [Header(">>>>>> Targets")]
        [SerializeField] protected GameObject selectedTarget;
        [SerializeField] protected VisibleSurroundings surroundings;
        
        private void Start() {
            thisUnit = GetComponent<NPCUnit>();
            base.thisUnit = thisUnit;
            base.Start();
        }

        private void FixedUpdate() {
            base.FixedUpdate();
            MonitorSurroundings();
            ActAccordingToRole();
        }
        
        
        private void MonitorSurroundings() {
            surroundings.Clear();
            
            foreach (GameObject obj in objectsAround) {
                ClassifyObject(obj);
            }
        }

        protected abstract void ClassifyObject(GameObject obj);

        private void ActAccordingToRole() {
            switch (thisUnit.role) {
                case UnitRole.Agressive_Patrolling:
                    // Do scenario 1
                    ExecuteAggressivePatrolling();
                    break;
                case UnitRole.Agressive_StayStill:
                    // Do scenario 2
                    ExecuteAggressiveStayStill();
                    break;
            }
        }

        private void ExecuteAggressivePatrolling() {
            // ...
            // Patrolling
            // ...
            
            SelectTarget();

            if (!selectedTarget)
                return;

            if (GoodAttackDistance(selectedTarget.transform.position)) {
                AttackTarget(selectedTarget);
            }
            else {
                FollowTarget(selectedTarget);
            }
        }

        private void ExecuteAggressiveStayStill() {
            // ...
            // Do nothing
            // ...

            SelectTarget();

            if (!selectedTarget)
                return;

            if (GoodAttackDistance(selectedTarget.transform.position)) {
                AttackTarget(selectedTarget);
            }
            else {
                FollowTarget(selectedTarget);
            }
        }
        
        private void SelectTarget() {
            if (selectedTarget) {
                // If we have the target specified and target is still valid
                if (surroundings.enemyUnits.Contains(selectedTarget)) 
                    return;
                
                // If target is no more valid, we null it
                selectedTarget = null;
                return;
            }

            if (!selectedTarget) {
                if (surroundings.enemyUnits.Count > 0) {
                    int randomIndex = Random.Range(0, surroundings.enemyUnits.Count);
                    selectedTarget = surroundings.enemyUnits[randomIndex];
                }
            }
        }
        
        private bool GoodAttackDistance(Vector2 targetPosition) {
            return Vector2.Distance(targetPosition, transform.position) < 2f;
        }

        protected abstract void AttackTarget(GameObject target);
        protected abstract void FollowTarget(GameObject target);
    }
    
    [Serializable]
    public struct VisibleSurroundings {
        [SerializeField] public List<GameObject> friendlyUnits;
        [SerializeField] public List<GameObject> enemyUnits;
        [SerializeField] public List<GameObject> consumables;

        public VisibleSurroundings(List<GameObject> friendlyUnits, List<GameObject> enemyUnits, List<GameObject> consumables) {
            this.friendlyUnits = friendlyUnits;
            this.enemyUnits = enemyUnits;
            this.consumables = consumables;
        }

        public void Clear() {
            friendlyUnits.Clear();
            enemyUnits.Clear();
            consumables.Clear();
        }

    }

    
}