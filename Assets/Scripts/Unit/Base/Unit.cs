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
    }
}