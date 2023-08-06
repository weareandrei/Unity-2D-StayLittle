using Unit.AI;
using UnityEngine;

namespace Unit.Controller {
    public class UnitController : BaseController {
        
        protected override void GetMovementInput() {
            Vector2Int pathfinderDirections = this.brain.GetDirectionInputs();
            moveDirection = pathfinderDirections.x;
            
            if (moveDirection != 0) {
                moveState = UnitMoveState.Moving;
            }
            else {
                moveState = UnitMoveState.Idle;
            }
        }

        protected override void GetMovementSpeed() {
            // Depending on other commands, or some factors Unit will speed up or slow down...
            return;
        }

        protected override void CheckInputs() {
            return;
        }
    }
}