using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Stats {
    
    [Serializable]
    public class PlayerStats : BaseStats, ICombatStats, ILevelProgressionStats {
        
        public float AttackRange { get; set; }
        public float BaseAttackDamage { get; set; }
        public int MaxXP { get; set; }
        public int CurrentXP { get; set; }
        
    }
    
}