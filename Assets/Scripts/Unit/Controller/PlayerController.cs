using UnityEngine;

namespace Unit.Controller {
    public class PlayerController : BaseController {
        
        [Header(">>>>>> Inputs")]
        [Space]

        [SerializeField] KeyCode run = KeyCode.LeftControl;
        [SerializeField] KeyCode jump = KeyCode.Space;
        // [SerializeField] KeyCode dash = KeyCode.LeftShift;
        // [SerializeField] KeyCode climb = KeyCode.Space;
        
        protected override void GetMovementInput() {
            // if (smoothMovement) MoveDirection = Input.GetAxis("Horizontal");
            // else MoveDirection = (Input.GetAxisRaw("Horizontal"));
            moveDirection = Input.GetAxisRaw("Horizontal");
            if (moveDirection != 0) {
                moveState = UnitMoveState.Moving;
            }
            else {
                moveState = UnitMoveState.Idle;
            }
        }

        protected override void GetMovementSpeed() {
            if (Input.GetKeyDown(run)) 
                moveState = UnitMoveState.Running;

            // if (Input.GetKeyUp(run))
            //     moveState = UnitMoveState.Moving;
        }

        protected override void CheckInputs() {
            if (Input.GetKeyDown(jump)) Jump();

            // // Dashing Input
            // if (Input.GetKeyDown(dash)) Dash();
            //
            // // Climbing Input
            // if (Input.GetKey(climb)) Climb();
        }

        protected override bool CanPerformJump() {
            throw new System.NotImplementedException();
        }

        private void Jump() {
            if (physicalState == UnitPhysicalState.Grounded) {
                AirJump();
            }
            if (physicalState == UnitPhysicalState.InAir) {
                AirJump();
            }
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
    }
}