using System.Collections.Generic;

namespace Unit.Stats {
    
    public interface ILevelProgressionStats {
        
        int MaxXP { get; set; }
        int CurrentXP { get; set; }
        // List<Ability> _abilitiesLearned { set; get; }
    }
    
}