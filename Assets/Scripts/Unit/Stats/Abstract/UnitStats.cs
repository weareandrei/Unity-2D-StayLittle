using System;
using UnityEngine;

namespace Unit.Stats {
    
    [Serializable]
    public abstract class UnitStats : StatsBase {
        
        [SerializeField] protected float _maxHP = 100f;
        [SerializeField] protected float _currentHP;

        [SerializeField] protected int _maxXP;
        [SerializeField] protected int _currentXP;
        [SerializeField] protected int _currentLevel;

        [SerializeField] protected float _isImmortal;
        
        [SerializeField] protected float _attackRange;

        [SerializeField] private UnitMoveStats _unitMoveStats;

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

        public int MaxXP {
            get { return _maxXP; }
            set { _maxXP = value; }
        }
        
        public int CurrentXP {
            get { return _currentXP; }
            set {
            _currentXP = Mathf.Clamp(value, 0, _maxXP);
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
        
        public float AttackRange {
            get { return _attackRange; }
            set { _attackRange = value; }
        }
        
        public UnitMoveStats UnitMoveStats {
            get { return _unitMoveStats; }
            set { _unitMoveStats = value; }
        }
        
    }
    
    [Serializable]
    public struct UnitMoveStats {
        public float speed;
        public float jumpForce;
        public bool allowDoubleJump;
        public float velocityThreshold;
        public float airControl;
    }
    
}