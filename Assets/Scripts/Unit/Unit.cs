using System;
using System.Collections.Generic;
using Actor.Base;
using UnityEngine;

namespace Unit.Base {
    public class Unit : MonoBehaviour {
        // public controller

        [SerializeField] private UnitStats stats;

        [SerializeField] public List<UnitTag> unitTags;

        private void Start() {
            stats.currentHP = stats.maxHP;
        }

        private void Update() {
            CheckSomething();
        }

        // private void FixedUpdate() {
        //     throw new NotImplementedException();
        // }
        
        private void CheckSomething() {
            if (stats.currentHP == 0) Die();
            return;
        }

        public bool TakeDamage(float damage) {
            float hpLeft = stats.currentHP - damage;
            if (hpLeft < 0) {
                hpLeft = 0;
            }

            stats.currentHP = hpLeft;
            return true;
        }

        private void Die() {
            gameObject.SetActive(false);
        }

    }
    
    public enum UnitTag {
        Player,
        Enemy,
        Friend
    }
}