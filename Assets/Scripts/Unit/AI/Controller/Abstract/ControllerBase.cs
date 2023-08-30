using System;
using Unit.Stats;
using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    
    public abstract class ControllerBase : MonoBehaviour {
        
        public BrainBase Brain { get; set; }
        
        private ActionBuffer actionBuffer = new ActionBuffer();
        
        [SerializeField] public UnitMoveStats unitMoveStats;

        private float groundedTimer = 0.27f;

        public Rigidbody2D rb;
        public BoxCollider2D col;
        
        // Current Body state
        protected int movementDirection;
        protected int currentJumps;
        protected bool isGrounded;
        protected bool isLookingRight;
        protected UnitPhysicalState physicalState;

        public bool MoveLock;
        public bool CombatLock;
        
        private void Update() {
            MonitorPhysicalState();
        }
        
        private void FixedUpdate() {
            ReadBuffer();
            Move();
        }

        private void ReadBuffer() {
            UnitAction nextAction;
            
            try { nextAction = actionBuffer.GetAction(); } 
            catch (Exception e) { return; }

            switch (nextAction) {
                case UnitAction.Jump:
                    Jump();
                    break;
                case UnitAction.MoveLeft:
                    SetMovementDirection(-1);
                    break;
                case UnitAction.MoveRight:
                    SetMovementDirection(1);
                    break;
                case UnitAction.StopMovement:
                    StopMovement();
                    break;
                case UnitAction.None:
                    break;
            }
        }

        private void Move() {
            float currentSpeed = unitMoveStats.speed;
            
            if (Mathf.Abs(rb.velocity.x) < unitMoveStats.velocityThreshold) {
                currentSpeed *= 5;
            }
            float horizontalForce = movementDirection * 1000 * currentSpeed * Time.fixedDeltaTime;
            Vector2 force = new Vector2(horizontalForce, 0f);

            if (physicalState == UnitPhysicalState.InAir) {
                force *= unitMoveStats.airControl;
            }

            rb.AddForce(force, ForceMode2D.Force);
            RotateToMoveDirection();
        }
        
        void RotateToMoveDirection() {
            if (movementDirection > 0) {
                if (isLookingRight == false) { } // OPTIONAL ANIMATION TURN RIGHT          (ANIMATION)

                isLookingRight = true;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            } 
            
            if (movementDirection < 0) {
                if (isLookingRight == true) { } // OPTIONAL ANIMATION TURN LEFT            (ANIMATION)

                isLookingRight = false;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
        
        private void MonitorPhysicalState() {
            CheckGrounded();
        }

        public void ReceiveActionRequest(UnitAction action) {
            actionBuffer.AddAction(action);
        } 
        
        void CheckGrounded() {
            RaycastHit2D ray;
            
            Vector2 position = new Vector2(col.bounds.center.x - col.bounds.extents.x, col.bounds.min.y);
            ray = Physics2D.Raycast(position, Vector2.down, col.bounds.extents.y/5, LayerMask.GetMask("Ground"));

            Debug.DrawRay(position, Vector2.down * (col.bounds.extents.y/5), Color.red);

            if (ray.collider != null) {
                if (groundedTimer <= 0f) {
                    physicalState = UnitPhysicalState.Grounded;
                    currentJumps = 0;
                } else {
                    groundedTimer -= Time.fixedDeltaTime;
                }
            } else {
                physicalState = UnitPhysicalState.InAir;
                groundedTimer = 0.1f;
            }
        }
        
        // ----- Abstract methods -----
        protected abstract void Jump();
        protected abstract void SetMovementDirection(int direction);
        protected abstract void StopMovement();
        // ----- ----- ----- ----- -----
    }
    
    [Serializable]
    public enum UnitPhysicalState {
        Grounded,
        InAir
    }
    
}