using System;
using System.Collections.Generic;
using Unit.AI;
using UnityEngine;

namespace Unit.Controller {
    public abstract class BaseController : MonoBehaviour {
        
        #region Variables
        
        // Movement
        [SerializeField] [Range(0, 1f)] protected float airControl = 0.75f;
        [SerializeField] protected float currentSpeed = 0f;
        [SerializeField] protected float moveDirection; // 1: y==1, 2: y==-1
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float runSpeed;
        [SerializeField] public List<UnitMovementActions> actionsAwaiting;
        
        // State
        private bool paused = false; // If you want to stop the game
        [SerializeField] protected UnitPhysicalState physicalState;
        [SerializeField] protected UnitMoveState moveState;
        [HideInInspector] public bool isLookingRight;
        [SerializeField] protected int currentJumps;
        
        // Physics
        public float maxJumpForce = 22;
        public int maxAirJumps = 1;
        public float gravity = 6;
        
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
            ray = Physics2D.Raycast(position, Vector2.down, unitCollider.bounds.extents.y + 0.02f);

            if (ray.collider != null) {
                physicalState = UnitPhysicalState.Grounded;
            } else {
                physicalState = UnitPhysicalState.InAir;
            }
        }
        
        #endregion
        
        #region Movement & Rotation
        protected abstract void AirJump();

        public abstract void Attack(GameObject attackTarget);

        void Move() {
            if (paused)
                return;

            currentSpeed = moveState switch {
                UnitMoveState.Running => runSpeed,
                UnitMoveState.Moving => moveSpeed,
                UnitMoveState.Idle => 0
            };

            if(physicalState == UnitPhysicalState.InAir)
                unitRigidbody.velocity = new Vector2(moveDirection * (airControl * currentSpeed) * Time.fixedDeltaTime, unitRigidbody.velocity.y);
            else
                unitRigidbody.velocity = new Vector2(moveDirection * currentSpeed * Time.fixedDeltaTime, unitRigidbody.velocity.y);
            
            RotateToMoveDirection();
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