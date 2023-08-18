using System;

namespace Actor.Base {
    
    [Serializable]
    public class UnitStats {
        public float maxHP = 100f;
        public float currentHP;
        public float currentXP;
        public float isImmortal;
    }
}