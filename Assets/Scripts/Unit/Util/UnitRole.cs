using System;

namespace Unit.Util {
    
    [Serializable]
    public enum UnitRole {
        DoNothing = 0,
        Agressive_Patrolling = 1,
        Agressive_StayStill = 2,
        Friendly_Patroling = 3,
        FriendlyStayStill = 4,
        MoveToTarget = 5
    }
}