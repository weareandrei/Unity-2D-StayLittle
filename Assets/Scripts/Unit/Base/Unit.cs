using System.Collections.Generic;
using Actor.Base;
using UnityEngine;

namespace Unit.Base {
    public abstract class Unit : MonoBehaviour {
        public UnitStats stats;
        [SerializeField] public List<UnitTag> unitTags;
        
        public abstract void RecieveDamage(float amount);
    }
    
    
    public enum UnitTag {
        Player,
        Enemy,
        Friend
    }
}