using Legacy.Unit_old.Controller.Base;
using UnityEngine;
using UnitPhysicalState = Legacy.Unit_old.Controller.Base.UnitPhysicalState;

namespace Legacy.Unit_old.Controller {
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

        protected override bool CanPerformJump() {
            if (physicalState == UnitPhysicalState.Grounded) {
                return true;
            }

            return false;
        }

        protected override void AirJump() {
            if (currentJumps >= maxAirJumps)
                return;

            // Here You add Air Jumping ANIMATION

            unitRigidbody.velocity = new Vector2(unitRigidbody.velocity.x, 0); // Reset vertical velocity

            Vector2 jumpForce = new Vector2(unitRigidbody.velocity.x * 2, maxJumpForce);
            unitRigidbody.AddForce(jumpForce, ForceMode2D.Impulse); // Apply jump force with added horizontal force
            currentJumps++;
        }

        public override void Attack(GameObject attackTarget) {
            brain.combatComponent.PerformAttack(attackTarget);
        }
        
        protected override void GetMovementSpeed() {
            throw new System.NotImplementedException();
        }

        protected override void CheckInputs() {
            throw new System.NotImplementedException();
        }
    }
}