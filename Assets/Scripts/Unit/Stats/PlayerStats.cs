using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Stats {
    
    [Serializable]
    public class PlayerStats : BaseStats, ICombatStats, ILevelProgressionStats {
        
        [SerializeField] private float attackRange;
        [SerializeField] private float baseAttackDamage;
        
        [SerializeField] private int maxXP;
        [SerializeField] private int currentXP;
        
        public float AttackRange {
            get { return attackRange; }
            set { attackRange = value; }
        }

        public float BaseAttackDamage {
            get { return baseAttackDamage; }
            set { baseAttackDamage = value; }
        }
        
        public int MaxXP {
            get { return maxXP; }
            set { maxXP = value; }
        }

        public int CurrentXP {
            get { return currentXP; }
            set { currentXP = value; }
        }

    }
    
}