using System.Collections.Generic;
using Unit.AI;
using UnityEngine;

namespace Unit.Controller {
    public class UnitController : BaseController {
        
        protected override void GetMovementInput() {
            Vector2Int pathfinderDirections = brain.GetDirectionInputs();
            if (brain.isPerformingAttack) {
                moveDirection = 0;
                moveState = UnitMoveState.Idle;
                return;
            }
            
            moveDirection = pathfinderDirections.x;
            
            if (moveDirection != 0) {
                moveState = UnitMoveState.Moving;
            }
            else {
                moveState = UnitMoveState.Idle;
            }

            actionsAwaiting.AddRange(brain.GetActionInputs());
        }

        protected override void GetMovementSpeed() {
            // Depending on other commands, or some factors Unit will speed up or slow down...
            return;
        }

        protected override void CheckInputs() {
            return;
        }

        protected override bool CanPerformJump() {
            if (physicalState == UnitPhysicalState.Grounded) {
                return true;
            }

            return false;
        }

        protected override void AirJump() {
            // if (currentJumps >= MaxAirJumps)
            //     return;

            // Here You add Air Jumping ANIMATION

            unitRigidbody.gravityScale = gravity;
            unitRigidbody.velocity = new Vector2(0,0);
            unitRigidbody.velocity = Vector2.up * maxJumpForce;
            currentJumps++;
        }

        public override void Attack(GameObject attackTarget) {
            brain.combatComponent.PerformAttack(attackTarget);
        }
    }
}