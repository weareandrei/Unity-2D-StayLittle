using Unit.Util;

namespace Unit.AI.Sensor {
    
    public class VisionSensor : ISensor {
        
        private BrainBase brain;

        public BrainBase Brain {
            get { return brain; }
            set { brain = value; }
        }

        // public SensorType Type { get; } = SensorType.Vision;
        
        public void ScanSurrounding() {
            throw new System.NotImplementedException();
        }
        
        public void SendSignalToBrain(string param) {
            throw new System.NotImplementedException();
        }
    }
    
}