using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    
    public interface ISensor : IBrainComponent {
        
        // SensorType Type { get; }

        void ScanSurrounding();
        
    }

}