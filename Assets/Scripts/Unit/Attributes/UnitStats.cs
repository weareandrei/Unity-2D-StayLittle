using System;
using UnityEngine;

namespace Unit.Base {

    [Serializable]
    public abstract class UnitStats {
        [SerializeField] protected float _maxHP = 100f;
        [SerializeField] protected float _currentHP;

        [SerializeField] protected int _currentLevel;

        [SerializeField] protected float _isImmortal;

        public float MaxHP {
            get { return _maxHP; }
            set { _maxHP = value; }
        }

        public float CurrentHP {
            get { return _currentHP; }
            set {
                _currentHP = Mathf.Clamp(value, 0, _maxHP);
            }
        }

        public int CurrentLevel {
            get { return _currentLevel; }
            set { _currentLevel = value; }
        }

        public float IsImmortal {
            get { return _isImmortal; }
            set { _isImmortal = value; }
        }
    }
}