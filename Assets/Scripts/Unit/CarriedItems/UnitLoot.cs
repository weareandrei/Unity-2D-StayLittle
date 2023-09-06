using System;
using System.Collections.Generic;
using Content.Base;
using UnityEngine;

namespace Unit.CarriedItems {
    
    [Serializable]
    public class UnitLoot {
        
        // DROPS
        [SerializeField]
        public int xpDrop;
        [SerializeField]
        public int moneyDrop;
        [SerializeField]
        public List<BaseContent> itemDrop;
    }
    
}