using System;
using UnityEngine;

namespace Unit.Stats {
    
    [Serializable]
    public class NPCUnitStats : BaseStats, ICombatStats {
        public float AttackRange { get; set; }
        public float BaseAttackDamage { get; set; }
    }
    
}