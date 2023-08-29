using UnityEngine;

namespace Legacy.Unit_old.Player {
    public class PlayerUnit : Legacy.Unit_old.Base.Unit {
        [SerializeField] public PlayerStats stats;
        
        private void Start() {
            stats.CurrentHP = stats.MaxHP;
            stats.CurrentLevel = 1;
            stats.CurrentXP = 0;
            stats.CurrentMoney = 0;
        }

        public override void RecieveDamage(float amount) {
            float currentHP_tmp = stats.CurrentHP;
            if (currentHP_tmp - amount <= 0f) {
                stats.CurrentHP = 0;
                Die();
            }

            stats.CurrentHP -= amount;
        }

        public void RecieveXP(int amount) {
            int currentXP_tmp = stats.CurrentXP;
            if (currentXP_tmp + amount >= stats.MaxXP) {
                stats.CurrentXP = currentXP_tmp - stats.MaxXP;
                LevelUp();
            }

            stats.CurrentXP += amount;
        }
        
        public void RecieveHP(float amount) {
            stats.CurrentHP += amount;
        }
        
        
        private void LevelUp() {
            stats.CurrentLevel++;
        }
        
        private void Die() {
            throw new System.NotImplementedException();
        }
    }
}