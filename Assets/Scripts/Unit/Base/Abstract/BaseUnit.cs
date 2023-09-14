using System;
using System.Collections.Generic;
using UnityEngine;
using Unit.Stats;
using Unit.Util;

namespace Unit.Base {
    
    [Serializable]
    public abstract class BaseUnit : MonoBehaviour {
        
        [SerializeField] public List<UnitTag> tags;
        [SerializeField] public BaseStats stats;

        protected void Awake() {
            stats.CurrentHP = stats.MaxHP;
            if (stats.CurrentLevel == 0) {
                stats.CurrentLevel = 1;
            }
        }

        public void RecieveDamage(float amount, GameObject source) {
            float currentHP_tmp = stats.CurrentHP;
            if (currentHP_tmp - amount <= 0f) {
                stats.CurrentHP = 0;
                Die(source);
            }

            stats.CurrentHP -= amount;
        }

        public void ReceiveHP(int amount, GameObject source) {
            stats.CurrentHP += amount;
        }

        protected abstract void Die(GameObject source);
    }
}