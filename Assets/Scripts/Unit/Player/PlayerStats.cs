using System.Collections.Generic;
using Actor.Base;
using Actor.Base.Component;

namespace Unit.Player {
    public class PlayerStats : UnitStats {
        private List<Ability> abilitiesLearned;
        
        public void AddXP(int amount) {
        
        }

        private void LevelUp() {
            currentLevel++;
        }
    
        public void AddHealth() {
        
        }
    
        public void AddMoney() {
        
        }
    }
}