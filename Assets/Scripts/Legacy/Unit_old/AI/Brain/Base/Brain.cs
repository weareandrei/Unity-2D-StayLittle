using System.Collections.Generic;
using Legacy.Unit_old.AI.Chat;
using Legacy.Unit_old.AI.Combat;
using Legacy.Unit_old.AI.Navigation;
using Legacy.Unit_old.AI.Vision;
using Legacy.Unit_old.Controller.Base;
using Unit.AI;
using UnityEngine;
using UnitPhysicalState = Legacy.Unit_old.Controller.Base.UnitPhysicalState;

namespace Legacy.Unit_old.AI.Brain.Base {
    public abstract class Brain : MonoBehaviour {
        
        protected Pathfinder pathfinder;
        public CombatComponent combatComponent;
        protected ChatComponent chatBrain;
        protected VisionComponent vision;
        protected BaseController controller;

        [Header(">>>>>> Unit's limitations")] [Space] 
        [SerializeField] private bool canMove;
        [SerializeField] private bool canSpeak;
        [SerializeField] private bool canFight;
        [SerializeField] protected float minimumFollowDistance = 1f;

        [Header(">>>>>> States")] [Space] 
        [SerializeField] private bool paused;
        [SerializeField] public bool isPerformingAttack;
        
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
            
            if (canSpeak) {
                chatBrain = new ChatComponent();
            }

            controller = GetComponent<BaseController>();
            vision = GetComponent<VisionComponent>();
            combatComponent = GetComponent<CombatComponent>();
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
        
        public List<UnitMovementActions> GetActionInputs() {
            // Do some extra check after getting commands from Pathfinder. 
            //    When we drive using GPS, we check what it says because it can be wrong sometimes right?
            List<UnitMovementActions> pathFinderActionCommands = pathfinder.GetAwaitingActions();
            
            for (int i = 0; i < pathFinderActionCommands.Count; i++) {
                if (pathFinderActionCommands[i] == UnitMovementActions.Jump && 
                    controller.physicalState == UnitPhysicalState.InAir) {
                    pathFinderActionCommands.RemoveAt(i);
                }
            }

            return pathFinderActionCommands;
        }
        
        public Vector2Int Get() {
            return pathfinder.currentDirection;
        }
        
        #endregion
        
        
    }
}