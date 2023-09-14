using Unit.Base;
using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    
    public class EnemyBrain : UnitBrain {

        // protected override void ReactToSurroundings() {
        //     switch (thisUnit.role) {
        //         case UnitRole.Agressive_Patroling:
        //             // Do scenario 1
        //             break;
        //         case UnitRole.Agressive_StayStill:
        //             // Do scenario 2
        //             break;
        //     }
        // }

        protected override void ClassifyObject(GameObject obj) {
           
            BaseUnit unitComponent = obj.GetComponent<BaseUnit>();

            if (!unitComponent) {
                return;
            }

            foreach (UnitTag tag in unitComponent.tags) {
                switch (tag) {
                    case UnitTag.Player:
                        surroundings.enemyUnits.Add(obj);
                        break;
                    case UnitTag.Child:
                        surroundings.enemyUnits.Add(obj);
                        break;
                    case UnitTag.Enemy:
                        surroundings.friendlyUnits.Add(obj);
                        break;
                }
            }

        }

        protected override void FollowTarget(GameObject target) {
            brainComponents.pathfinder.SelectTarget(target);
        }
        
        protected override void AttackTarget(GameObject target) {
            if (!brainComponents.combatComponent.isPerformingAttack) {
                brainComponents.combatComponent.PerformAttack("LightAttack");
            }
        }

    }
    
}