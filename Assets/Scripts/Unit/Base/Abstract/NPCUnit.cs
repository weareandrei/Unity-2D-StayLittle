using Unit.CarriedItems;
using Unit.Stats;
using Unit.Util;
using UnityEngine;

namespace Unit.Base {
    
    public class NPCUnit : BaseUnit {
        
        [SerializeField] public UnitRole role;
        [SerializeField] public UnitLoot unitLoot;
        
        [SerializeField] public NPCUnitStats stats;

        protected void Awake() {
            base.stats = stats;
            base.Awake();
        }

        protected override void Die(GameObject killer) {
            BaseUnit killerUnit = killer.GetComponent<BaseUnit>();
    
            if (killerUnit is PlayerUnit) {
                PlayerUnit playerKiller = killerUnit as PlayerUnit;
                playerKiller.ReceiveXP(unitLoot.xpDrop);
            }
            
            // Continue with other logic if necessary
            Destroy(gameObject);
        }

    }
    
}