using System;
using Unit.Controller;
using UnityEngine;

namespace Unit.AI {
    public abstract class Brain : MonoBehaviour {
        
        protected Pathfinder pathfinder;
        protected CombatComponent combatBrain;
        protected ChatComponent chatBrain;
        protected VisionComponent vision;
        protected BaseController controller;

        [Header(">>>>>> Unit's limitations")] [Space] 
        [SerializeField] private bool canMove;
        [SerializeField] private bool canSpeak;
        [SerializeField] private bool canFight;


        [Header(">>>>>> States")] [Space] 
        [SerializeField] private bool paused;
        
        [Header(">>>>>> Targets")] [Space] 
        [SerializeField] protected GameObject attackTarget;
        [SerializeField] protected GameObject followTarget;
        
        public abstract void SelectAttackTarget(GameObject target);
        public abstract void SelectFollowTarget(GameObject target);
        
        #region Action Methods
        
        public abstract void Follow();
        
        public abstract void Attack();
        
        #endregion
        
        #region Decision Making Methods
        
        public abstract void ReactToObjectsAround();
        
        #endregion
        
        #region Unity Functions
        
        private void Start() {
            if (canMove) {
                pathfinder = gameObject.AddComponent<Pathfinder>();
            }

            if (canFight) {
                combatBrain = new CombatComponent();
            }

            if (canSpeak) {
                chatBrain = new ChatComponent();
            }

            controller = GetComponent<BaseController>();
            vision = GetComponent<VisionComponent>();
        }

        void Update() {
            if (paused)
                return;
            
            vision.DetectObjectsAround();
            ReactToObjectsAround();
        }
        
        void FixedUpdate() {
            Follow();
            Attack();
        }
        
        #endregion
        
        #region Getters & Setters

        public Vector2Int GetDirectionInputs() {
            return pathfinder.currentDirection;
        }
        
        #endregion
        
        
    }
}