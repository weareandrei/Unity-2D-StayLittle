using UnityEngine;

namespace Unit.Enemy {
    public class EnemyUnit : Base.Unit {
        [SerializeField] public EnemyStats stats;
        
        private void Start() {
            stats.CurrentHP = stats.MaxHP;
            stats.CurrentLevel = 1; // todo: Select Randomly from certain range
        }
        
        public override void RecieveDamage(float amount) {
            float currentHP_tmp = stats.CurrentHP;
            if (currentHP_tmp - amount <= 0f) {
                stats.CurrentHP = 0;
                Die();
            }

            stats.CurrentHP -= amount;
        }
        
        public void RecieveHP(float amount) {
            stats.CurrentHP += amount;
        }

        private void Die() {
            throw new System.NotImplementedException();
        }

    }
}