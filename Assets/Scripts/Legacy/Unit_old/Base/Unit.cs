using System.Collections.Generic;
using Legacy.Unit_old.Attributes;
using UnityEngine;

namespace Legacy.Unit_old.Base {
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