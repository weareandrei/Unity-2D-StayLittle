using System.Collections.Generic;
using UnityEngine;

namespace Unit.AI {
    public class EnemyBrain : Brain {

        public override void SelectAttackTarget(GameObject target) {
            this.attackTarget = target;
        }

        public override void SelectFollowTarget(GameObject target) {
            this.followTarget = target;
        }

        public override void Follow() {
            if (followTarget) {
                pathfinder.SelectTarget(followTarget);
            }
        }

        public override void Attack() {
            return;
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