using Unit.Stats;
using UnityEngine;

namespace Unit.Base {
    public class PlayerUnit : BaseUnit {
        
        [SerializeField] public PlayerStats stats;
        
        private new void Awake() {
            base.stats = stats; 
            base.Awake();
        }
        
        public void ReceiveXP(int amount) {
            int currentXP_tmp = stats.CurrentXP;
            if (currentXP_tmp + amount >= stats.MaxXP) {
                stats.CurrentXP = currentXP_tmp - stats.MaxXP;
                LevelUp();
            }
        
            stats.CurrentXP += amount;
        }
        
        private void LevelUp() {
            stats.CurrentLevel++;
        }

        protected override void Die(GameObject source) {
            throw new System.NotImplementedException();
        }
    }
}