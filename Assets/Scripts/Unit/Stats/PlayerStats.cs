using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Stats {
    
    [Serializable]
    public class PlayerStats : UnitStats {
        
        protected int _currentXP;
        protected int _maxXP;

        protected float _currentMoney;
        // protected List<Ability> _abilitiesLearned;

        public int CurrentXP {
            get { return _currentXP; }
            set {
            _currentXP = Mathf.Clamp(value, 0, _maxXP);
            }
        }

        public int MaxXP {
            get { return _maxXP; }
            set { _maxXP = value; }
        }
        
        public float CurrentMoney {
            get { return _currentMoney; }
            set { _currentMoney = value; }
        }

    }
    
}