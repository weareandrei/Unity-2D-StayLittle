using Legacy.Unit_old.Controller.Base;
using UnityEngine;

namespace Legacy.Unit_old.Controller {
    public class PlayerController : BaseController {
        
        [Header(">>>>>> Inputs")]
        [Space]

        [SerializeField] KeyCode run = KeyCode.LeftControl;
        [SerializeField] KeyCode jump = KeyCode.Space;
        [SerializeField] KeyCode attack = KeyCode.E;
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
            if (Input.GetKeyDown(attack)) Attack(null);

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

        public override void Attack(GameObject attackTarget) {
            brain.combatComponent.PerformAttack(attackTarget);
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
    }
}