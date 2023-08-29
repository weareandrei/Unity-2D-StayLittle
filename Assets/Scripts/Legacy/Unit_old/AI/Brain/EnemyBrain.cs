using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Legacy.Unit_old.AI.Brain {
    public class EnemyBrain : Base.Brain {

        public override void SelectAttackTarget(GameObject target) {
            this.attackTarget = target;
        }

        public override void SelectFollowTarget(GameObject target) {
            this.followTarget = target;
        }

        public override void Follow() {
            if (followTarget && !GoodAttackDistance(followTarget.transform.position)) {
                pathfinder.SelectTarget(followTarget);
            }
        }

        public override void Attack() {
            if (attackTarget && GoodAttackDistance(attackTarget.transform.position) && !isPerformingAttack) {
                controller.Attack(attackTarget);
            }
        }

        private bool GoodAttackDistance(Vector2 targetPosition) {
            float distance = Vector2.Distance(
                transform.position,
                targetPosition);

            return distance < minimumFollowDistance;
        }
        
        public override void ReactToObjectsAround() {
            List<GameObject> objectsAround = vision.GetObjectsAround();

            foreach (GameObject thisObject in objectsAround) {
                if (thisObject.tag == "Player") {
                    SelectFollowTarget(thisObject);
                    SelectAttackTarget(thisObject);
                }
            }
        }
    }
}