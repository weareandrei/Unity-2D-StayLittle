using UnityEngine;

namespace Unit.AI {
    public abstract class SensorBase : MonoBehaviour, ISensor{
        
        private BrainBase brain;

        public BrainBase Brain {
            get { return brain; }
            set { brain = value; }
        }
        
        private void FixedUpdate() {
            ScanSurrounding();
        }

        public abstract void SendSignalToBrain(string param = null);
        public abstract void ScanSurrounding();
    }
}