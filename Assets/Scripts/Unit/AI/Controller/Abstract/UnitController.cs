using UnityEngine;

namespace Unit.AI {
    
    public class UnitController : ControllerBase {
        protected override void Jump() {
            if (physicalState == UnitPhysicalState.InAir) {
                if (currentJumps == 1 && !unitMoveStats.allowDoubleJump) 
                    return;
            }
            
            rb.velocity = new Vector2(rb.velocity.x, 0);

            Vector2 jumpForce = new Vector2(rb.velocity.x * 2, unitMoveStats.jumpForce);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
            currentJumps++;
        }

        protected override void SetMovementDirection(int direction) {
            movementDirection = direction;
        }

        protected override void StopMovement() {
            movementDirection = 0;
        }
    }
    
}