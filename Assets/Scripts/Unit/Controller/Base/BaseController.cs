using System;
using System.Collections.Generic;
using Unit.AI;
using UnityEngine;

namespace Unit.Controller {
    public abstract class BaseController : MonoBehaviour {
        
        #region Variables
        
        // Movement
        [SerializeField] [Range(0, 2f)] protected float airControl = 2f;
        [SerializeField] protected float currentSpeed = 0f;
        [SerializeField] protected float moveDirection; // 1: y==1, 2: y==-1
        
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float runSpeed;
        
        [SerializeField] public List<UnitMovementActions> actionsAwaiting;
        private float groundedTimer = 0.4f;
        
        [SerializeField] public float minimumVelocityThreshold = 0.2f;
        [SerializeField] public float initialForceImpulse = 0.7f;

        // State
        private bool paused = false; // If you want to stop the game
        [SerializeField] protected UnitPhysicalState physicalState;
        [SerializeField] protected UnitMoveState moveState;
        [HideInInspector] public bool isLookingRight;
        [SerializeField] protected int currentJumps;

        // Physics
        public float maxJumpForce = 22;
        public int maxAirJumps = 1;
        [SerializeField] protected float movementDampingFactor = 0.5f;

        
        // Physics components
        protected Rigidbody2D unitRigidbody;
        protected BoxCollider2D unitCollider;

        protected Brain brain;
        
        #endregion
        
        #region Unity Functions
        void Start() {
            GetComponenets();

            actionsAwaiting = new List<UnitMovementActions>();
        }  
        
        void Update() {
            if (paused)
                return;

            GetMovementInput();
            GetMovementSpeed();
            CheckInputs();
        }
        
        void FixedUpdate() {
            Move();
            CheckGrounded();
            PerformAction();
        }
        #endregion
        
        #region Set Movement
        protected abstract void GetMovementInput();
        protected abstract void GetMovementSpeed();
        #endregion
        
        #region Checks
        protected abstract void CheckInputs();
        protected abstract bool CanPerformJump();
        void CheckGrounded() {
            RaycastHit2D ray;
            
            Vector2 position = new Vector2(unitCollider.bounds.center.x - unitCollider.bounds.extents.x, unitCollider.bounds.min.y);
            ray = Physics2D.Raycast(position, Vector2.down, unitCollider.bounds.extents.y/5, LayerMask.GetMask("Ground"));

            Debug.DrawRay(position, Vector2.down * (unitCollider.bounds.extents.y/5), Color.red);

            if (ray.collider != null) {
                if (groundedTimer <= 0f) {
                    physicalState = UnitPhysicalState.Grounded;
                    currentJumps = 0;
                } else {
                    groundedTimer -= Time.fixedDeltaTime;
                }
            } else {
                physicalState = UnitPhysicalState.InAir;
                groundedTimer = 0.4f;
            }
        }
        
        #endregion
        
        #region Movement & Rotation
        protected abstract void AirJump();

        public abstract void Attack(GameObject attackTarget);

        protected void Move() {
            if (paused)
                return;

            currentSpeed = moveState switch {
                UnitMoveState.Running => runSpeed,
                UnitMoveState.Moving => moveSpeed,
                UnitMoveState.Idle => 0
            };

            float horizontalForce = moveDirection * currentSpeed * Time.fixedDeltaTime;
            Vector2 force = new Vector2(horizontalForce, 0f);

            if (physicalState == UnitPhysicalState.InAir) {
                force *= airControl;
            }
            
            if (Mathf.Abs(unitRigidbody.velocity.x) < minimumVelocityThreshold) {
                // If velocity is below the threshold, apply additional force to start movement instantly
                unitRigidbody.AddForce(new Vector2(moveDirection * initialForceImpulse, 0f), ForceMode2D.Impulse);
            }
    
            unitRigidbody.AddForce(force, ForceMode2D.Force);
            ApplyDamping(); // Apply damping force to gradually stop movement
            RotateToMoveDirection();
        }

        
        protected void ApplyDamping() {
            // Apply a damping force to counteract movement over time
            Vector2 dampingForce = -unitRigidbody.velocity * movementDampingFactor;
            unitRigidbody.AddForce(dampingForce, ForceMode2D.Force);
        }


        void RotateToMoveDirection() {
 
            if (moveDirection > 0) {
                if (isLookingRight == false) { } // OPTIONAL ANIMATION TURN RIGHT          (ANIMATION)

                isLookingRight = true;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            } 
            
            if (moveDirection < 0) {
                if (isLookingRight == true) { } // OPTIONAL ANIMATION TURN LEFT            (ANIMATION)

                isLookingRight = false;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }

        void PerformAction() {
            foreach (UnitMovementActions action in actionsAwaiting) {
                switch (action) {
                    case UnitMovementActions.Jump:
                        if (CanPerformJump()) {
                            actionsAwaiting.Remove(action);
                            AirJump();
                            return;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion
        
        void GetComponenets() {
            unitRigidbody = GetComponent<Rigidbody2D>();
            unitCollider = GetComponent<BoxCollider2D>();
            brain = GetComponent<Brain>();
        }

    }

    [Serializable]
    public enum UnitPhysicalState {
        Grounded,
        InAir
    }
    
    [Serializable]
    public enum UnitMoveState {
        Idle,
        Moving,
        Running
    }
    
    public enum UnitMovementActions {
        Jump
    }
}