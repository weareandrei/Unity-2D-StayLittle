using System;
using UnityEngine;

namespace Unit.AI {
    public abstract class Brain : MonoBehaviour {
        private Pathfinder pathfinder;
        private CombatComponent combatBrain;
        private ChatComponent chatBrain;

        [Header(">>>>>> Unit's limitations")] [Space] 
        [SerializeField] private bool canMove;
        [SerializeField] private bool canSpeak;
        [SerializeField] private bool canFight;

        public abstract void AttackTarget(GameObject target);
        public abstract void FollowTarget(GameObject target);

        private void Start() {
            if (canMove) {
                pathfinder = new Pathfinder();
            }

            if (canFight) {
                combatBrain = new CombatComponent();
            }

            if (canSpeak) {
                chatBrain = new ChatComponent();
            }
        }
    }
}