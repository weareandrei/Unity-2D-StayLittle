using System;
using System.Collections.Generic;
using UnityEngine;
using Unit.Stats;
using Unit.Util;

namespace Unit.Base {
    
    [Serializable]
    public class Unit : MonoBehaviour {
        [SerializeField] public UnitRole role;
        [SerializeField] public List<UnitTag> tags;

        [Header(" ----- Unit Stats ----- ")]
        [Space]
        [SerializeField] public UnitStats stats;

        private void Start() {
            
            stats.CurrentHP = stats.MaxHP;
            stats.CurrentLevel = 1;
            stats.CurrentXP = 0;
            // stats.CurrentMoney = 0;
        }

        public void RecieveDamage(float amount) {
            float currentHP_tmp = stats.CurrentHP;
            if (currentHP_tmp - amount <= 0f) {
                stats.CurrentHP = 0;
                Die();
            }

            stats.CurrentHP -= amount;
        }

        public void RecieveXP(int amount) {
            int currentXP_tmp = stats.CurrentXP;
            if (currentXP_tmp + amount >= stats.MaxXP) {
                stats.CurrentXP = currentXP_tmp - stats.MaxXP;
                LevelUp();
            }

            stats.CurrentXP += amount;
        }
        
        public void RecieveHP(float amount) {
            stats.CurrentHP += amount;
        }
        
        
        private void LevelUp() {
            stats.CurrentLevel++;
        }
        
        private void Die() {
            throw new System.NotImplementedException();
        }
    }
}