using Unit.Util;

namespace Unit.AI {
    
    public interface ISensor : IBrainComponent {
        
        // SensorType Type { get; }
        
        void ScanSurrounding();
        
    }

}