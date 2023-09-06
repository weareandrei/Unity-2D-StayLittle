using System.Collections.Generic;

namespace Unit.Stats {
    
    public interface ILevelProgressionStats {
        int MaxXP { set; get; }
        int CurrentXP { set; get; }
        // List<Ability> _abilitiesLearned { set; get; }
    }
    
}